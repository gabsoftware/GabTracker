using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GabTracker
{
    /// <summary>
    /// Represents the data for a line of GabTracker
    /// </summary>
    public class GabTrackerFeed
    {
        internal Color _fillcolor = Color.YellowGreen;

        /// <summary>
        /// Retrieve the Pen used to draw the line.
        /// </summary>
        [Browsable(false)]
        internal Pen LinePen
        {
            get
            {
                Pen p = new Pen(_linecolor, _linethickness);
                p.DashStyle = _linedashstyle;
                return p;
            }
        }

        /// <summary>
        /// Retrieve the Brush used to fill under the line.
        /// </summary>
        [Browsable(false)]
        internal SolidBrush FillBrush
        {
            get
            {
                return new SolidBrush(_fillcolor);
            }
        }

        private Color _linecolor = Color.YellowGreen;
        /// <summary>
        /// The color of the line.
        /// </summary>
        [Category("Feed Line")]
        [Browsable(true)]
        [Description("The color of the line.")]
        [DefaultValue(typeof(Color), "YellowGreen")]
        public Color LineColor
        {
            get
            {
                return _linecolor;
            }
            set
            {
                _linecolor = value;
                _fillcolor = Color.FromArgb(_fillalpha, value);
            }
        }

        private float _linethickness = 1;
        /// <summary>
        /// The thickness of the line.
        /// </summary>
        [Category("Feed Line")]
        [Description("The thickness of the line.")]
        [Browsable(true)]
        [DefaultValue(1)]
        public float LineThickness
        {
            get
            {
                return _linethickness;
            }
            set
            {
                _linethickness = value;
            }
        }

        private float _fillthickness = 1;
        /// <summary>
        /// The thickness of the fill. If you set this value to GabTracker.GridIntervalX, the fill will be uniform.
        /// </summary>
        [Category("Feed Fill")]
        [Description("The thickness of the fill. If you set this value to GabTracker.GridIntervalX, the fill will be uniform.")]
        [Browsable(true)]
        [DefaultValue(1)]
        public float FillThickness
        {
            get
            {
                return _fillthickness;
            }
            set
            {
                _fillthickness = value;
            }
        }

        private DashStyle _linedashstyle = DashStyle.Solid;
        /// <summary>
        /// The dash style of the line.
        /// </summary>
        [Category("Feed Line")]
        [Description("The dash style of the line.")]
        [Browsable(true)]
        [DefaultValue(DashStyle.Solid)]
        public DashStyle LineDashStyle
        {
            get
            {
                return _linedashstyle;
            }
            set
            {
                _linedashstyle = value;
            }
        }

        private DashStyle _filldashstyle = DashStyle.Solid;
        /// <summary>
        /// The dash style of the fill.
        /// </summary>
        [Category("Feed Fill")]
        [Description("The dash style of the fill.")]
        [Browsable(true)]
        [DefaultValue(DashStyle.Solid)]
        public DashStyle FillDashStyle
        {
            get
            {
                return _filldashstyle;
            }
            set
            {
                _filldashstyle = value;
            }
        }

        private Queue<double> _data = new Queue<double>();

        /// <summary>
        /// The data of the line.
        /// </summary>
        [Category("Feed")]
        [Browsable(false)]
        public Queue<double> Data
        {
            get
            {
                return _data;
            }
        }

        private bool _inverted = false;
        /// <summary>
        /// Set to true to invert the Y axis of the line.
        /// </summary>
        [Category("Feed")]
        [Description("Set to true to invert the Y axis of the line.")]
        [Browsable(true)]
        [DefaultValue(false)]
        public bool Inverted
        {
            get
            {
                return _inverted;
            }
            set
            {
                _inverted = value;
            }
        }

        private bool _fillunder = false;
        /// <summary>
        /// Set to true to fill under the line.
        /// </summary>
        [Category("Feed Fill")]
        [Description("Set to true to fill under the line.")]
        [Browsable(true)]
        [DefaultValue(false)]
        public bool FillUnder
        {
            get
            {
                return _fillunder;
            }
            set
            {
                _fillunder = value;
            }
        }

        private bool _fillsuperpose = false;
        /// <summary>
        /// Set to true if the line fill should not stop when encountering another line below. Set to false to fill until another line is encountered.
        /// </summary>
        [Category("Feed Fill")]
        [Description("Set to true if the line fill should not stop when encountering another line below. Set to false to fill until another line is encountered.")]
        [Browsable(true)]
        [DefaultValue(false)]
        public bool FillSuperpose
        {
            get
            {
                return _fillsuperpose;
            }
            set
            {
                _fillsuperpose = value;
            }
        }

        private bool _fillstopotherfeeds = true;
        /// <summary>
        /// Set to true if the line should stop the fill of the lines above, false otherwise. Setting to false allows to bypass the FillSuperpose property.
        /// </summary>
        [Category("Feed Fill")]
        [Description("Set to true if the line should stop the fill of the lines above, false otherwise. Setting to false allows to bypass the FillSuperpose property.")]
        [Browsable(true)]
        [DefaultValue(true)]
        public bool FillStopOtherFeeds
        {
            get
            {
                return _fillstopotherfeeds;
            }
            set
            {
                _fillstopotherfeeds = value;
            }
        }

        private Byte _fillalpha = 255;
        /// <summary>
        /// Get or set the alpha value of the line fill.
        /// </summary>
        [Category("Feed Fill")]
        [Description("Get or set the alpha value of the line fill.")]
        [Browsable(true)]
        [DefaultValue(255)]
        public Byte FillAlpha
        {
            get
            {
                return _fillalpha;
            }
            set
            {
                _fillalpha = value;
                _fillcolor = Color.FromArgb(value, _linecolor);
            }
        }

        private double _value = 0;
        /// <summary>
        /// Get or set the current value of the line.
        /// </summary>
        [Category("Feed")]
        [Description("Get or set the current value of the line.")]
        [Browsable(true)]
        [DefaultValue(1)]
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        private double _coefficient = 1;
        /// <summary>
        /// Get or set the coefficient used to draw the line. Example : a line with value of 50 and a coefficient of 0.5 will be drawn as if tha value was 25.
        /// </summary>
        [Category("Feed")]
        [Description("Get or set the coefficient used to draw the line. Example : a line with value of 50 and a coefficient of 0.5 will be drawn as if tha value was 25.")]
        [Browsable(true)]
        [DefaultValue(1)]
        public double Coefficient
        {
            get
            {
                return _coefficient;
            }
            set
            {
                _coefficient = value;
            }
        }

        private string _unit = String.Empty;
        /// <summary>
        /// Get or set a string representing the unit of the line.
        /// </summary>
        [Category("Feed")]
        [Browsable(true)]
        [Description("Get or set a string representing the unit of the line.")]
        public string Unit
        {
            get
            {
                return _unit;
            }
            set
            {
                _unit = value;
            }
        }

        private string _legend = String.Empty;
        /// <summary>
        /// Get or set a string representing the legend of the line.
        /// </summary>
        [Category("Feed")]
        [Description("Get or set a string representing the legend of the line.")]
        [Browsable(true)]
        public string Legend
        {
            get
            {
                return _legend;
            }
            set
            {
                _legend = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of GabTrackerFeed.
        /// </summary>
        public GabTrackerFeed()
        {

        }

        /// <summary>
        /// Initializes a new instance of GabTrackerFeed.
        /// </summary>
        public GabTrackerFeed(
            Color color,
            float thickness = 1f,
            bool inverted = false,
            double coefficient = 1.0,
            string unit = "",
            DashStyle dashstyle = DashStyle.Solid,
            bool fillunder = false,
            byte fillalpha = 255,
            float fillthickness = 1f,
            DashStyle filldashstyle = DashStyle.Solid,
            bool fillsuperpose = false,
            string legend = "",
            bool fillstopotherfeeds = true)
        {
            LineColor = color;
            LineThickness = thickness;
            Inverted = inverted;
            Coefficient = coefficient;
            Unit = unit;
            LineDashStyle = dashstyle;
            FillUnder = fillunder;
            FillAlpha = fillalpha;
            FillThickness = fillthickness;
            FillDashStyle = filldashstyle;
            FillSuperpose = fillsuperpose;
            Legend = legend;
            FillStopOtherFeeds = fillstopotherfeeds;
        }
    }
}
