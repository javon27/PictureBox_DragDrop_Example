using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Drag_and_Drop
{
    public partial class Form1 : Form
    {
        PictureBox[] boxes;
        PictureBox selected;

        public Form1()
        {
            InitializeComponent();

            boxes = new PictureBox[] { pictureBox1, pictureBox2, pictureBox3, pictureBox4 };

            try
            {
                // Supply your own images here
                // Sample images downloaded from https://www.freeimages.com
                pictureBox1.BackgroundImage = Image.FromFile(@"Images\image1.jpg");
                pictureBox2.BackgroundImage = Image.FromFile(@"Images\image2.jpg");
            }
            catch (Exception)
            {
                MessageBox.Show("No images found", "No Images Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var box in boxes)
            {
                box.AllowDrop = true;
                box.DragDrop += PictureBox_DragDrop;
                box.DragEnter += PictureBox_DragEnter;
                box.MouseClick += PictureBox_MouseClick;
                box.MouseMove += PictureBox_MouseMove;
                box.Paint += PictureBox_Paint;
            }
        }

        /// <summary>
        /// Fires after dragging has completed on the target control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox_DragDrop (object sender, DragEventArgs e)
        {
            var target = (PictureBox)sender;
            if (e.Data.GetDataPresent(typeof(PictureBox)))
            {
                var source = (PictureBox)e.Data.GetData(typeof(PictureBox));
                if (source != target)
                {
                    Console.WriteLine("Do DragDrop from " + source.Name + " to " + target.Name);
                    // You can swap the images out, replace the target image, etc.
                    SwapImages(source, target);

                    SelectBox(target);
                    return;
                }
            }
            Console.WriteLine("Don't do DragDrop");
        }

        /// <summary>
        /// Set the target's accepted DragDropEffect. Should match the source.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox_DragEnter (object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        /// <summary>
        /// Handle mouse click on picture box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            SelectBox((PictureBox)sender);
        }

        /// <summary>
        /// Only start DragDrop if the mouse moves. Allows MouseClick to trigger
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var pb = (PictureBox)sender;
                if (pb.BackgroundImage != null)
                {
                    pb.DoDragDrop(pb, DragDropEffects.Move);
                }
            }
        }

        /// <summary>
        /// Override paint so we can draw a border on a selected image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            var pb = (PictureBox)sender;
            pb.BackColor = Color.White;
            if (selected == pb)
            {
                ControlPaint.DrawBorder(e.Graphics, pb.ClientRectangle,
                   Color.Blue, 5, ButtonBorderStyle.Solid,  // Left
                   Color.Blue, 5, ButtonBorderStyle.Solid,  // Top
                   Color.Blue, 5, ButtonBorderStyle.Solid,  // Right
                   Color.Blue, 5, ButtonBorderStyle.Solid); // Bottom
            }
        }

        /// <summary>
        /// Set the selected image, and trigger repaint on all boxes.
        /// </summary>
        /// <param name="pb"></param>
        private void SelectBox(PictureBox pb)
        {
            if (selected != pb)
            {
                selected = pb; 
            }
            else
            {
                selected = null;
            }

            // Cause each box to repaint
            foreach (var box in boxes) box.Invalidate();
        }

        /// <summary>
        /// Swap images between two PictureBoxes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        private void SwapImages (PictureBox source, PictureBox target)
        {
            if (source.BackgroundImage == null && target.BackgroundImage == null)
            {
                return;
            }

            var temp = target.BackgroundImage;
            target.BackgroundImage = source.BackgroundImage;
            source.BackgroundImage = temp;
        }
    }
}
