using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;

namespace AceJump
{
    /// <summary>
    /// Adornment class that draws a square box in the top right hand corner of the viewport
    /// </summary>
    class AceJump
    {
        private Image _image;
        private IWpfTextView _view;
        private IAdornmentLayer _adornmentLayer;

        /// <summary>
        /// Creates a square image and attaches an event handler to the layout changed event that
        /// adds the the square in the upper right-hand corner of the TextView via the adornment layer
        /// </summary>
        /// <param name="view">The <see cref="IWpfTextView"/> upon which the adornment will be drawn</param>
        public AceJump(IWpfTextView view)
        {
            _view = view;


            Brush brush = new SolidColorBrush(Colors.BlueViolet);
            brush.Freeze();
            Brush penBrush = new SolidColorBrush(Colors.Red);
            penBrush.Freeze();
            Pen pen = new Pen(penBrush, 0.5);
            pen.Freeze();

            //draw a square with the created brush and pen
            System.Windows.Rect r = new System.Windows.Rect(0, 0, 30, 30);
            Geometry g = new RectangleGeometry(r);
            GeometryDrawing drawing = new GeometryDrawing(brush, pen, g);
            drawing.Freeze();

            DrawingImage drawingImage = new DrawingImage(drawing);
            drawingImage.Freeze();

            _image = new Image();
            _image.Source = drawingImage;

            //Grab a reference to the adornment layer that this adornment should be added to
            _adornmentLayer = view.GetAdornmentLayer("AceJump");

            //_view.ViewportHeightChanged += delegate { this.onSizeChange(); };
            //_view.ViewportWidthChanged += delegate { this.onSizeChange(); };

            _view.Caret.PositionChanged += delegate { this.onSizeChange(); };
        }

        public void onSizeChange()
        {
            //clear the adornment layer of previous adornments
            _adornmentLayer.RemoveAllAdornments();

            //Place the image in the top right hand corner of the Viewport
            Canvas.SetLeft(_image, _view.Caret.Top -20);
            Canvas.SetTop(_image, _view.Caret.Bottom +30);

            //add the image to the adornment layer and make it relative to the viewport
            _adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, _image, null);
        }
    }
}
