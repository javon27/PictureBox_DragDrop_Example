# PictureBox_DragDrop_Example
C# WinForms Example showing how to implement Drag &amp; Drop between PictureBox controls.

<img src="drag_drop_screenshot.png" width=400/>

## PictureBox and AllowDrop
Oddly enough, you won't see this property in Design View, or in IntelliSense. However, it is a valid property:

```C#
  PictureBox box = new PictureBox();
  box.AllowDrop = true;
```

## Implementing Drag & Drop
We need to handle 3 events for each PictureBox:

```C#
  box.DragDrop += PictureBox_DragDrop;
  box.DragEnter += PictureBox_DragEnter;
  box.MouseMove += PictureBox_MouseMove;
```

`DragDrop` is called when a user has finished dragging to a control. This is where you would put any logic on what you need to do.

`DragEnter` is where we broadcast what kind of `DragDropEffects` our target control can accept.

`MouseMove` is where we start the DragDrop action, not `MouseDown`. The reason for this is that if we call `box.DoDragDrop()` 
inside of `MouseDown`, the `MouseClick` event will never trigger. We only want DragDrop to start when the user moves the mouse.
