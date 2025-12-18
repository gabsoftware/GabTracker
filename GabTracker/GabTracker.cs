using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GabTracker
{
    /// <summary>
    /// GabTracker is a control used to mimic the Graph control used in the Task Manager of Windows.
    /// </summary>    
    [Description("GabTracker is a control used to mimic the Graph control used in the Task Manager of Windows.")]
    [ToolboxBitmap(typeof(GabTracker), "Resources.icon_16x16.png")]
    public partial class GabTracker : UserControl
    {
        private SolidBrush _backbrush = new SolidBrush(Color.Black);
        private Pen _gridpen;
        private Pen _gridthickerpen;
        private double[] _arrdata = { };
        private double _gridoffsetx = 0;
        private double[] _arrtmp = { };
        bool _haserror = false;
        private static readonly Color DefaultGridColor = Color.FromArgb(0, 75, 0);
        private static readonly Color DefaultGridThickerColor = Color.FromArgb(0, 75, 0);

        private static void EnsureBufferCapacity(ref double[] buffer, int requiredSize)
        {
            if (buffer == null || buffer.Length < requiredSize)
            {
                buffer = new double[requiredSize];
            }
        }

        private static int CopyFeedData(Queue<double> source, ref double[] target)
        {
            if (source == null || source.Count == 0)
            {
                return 0;
            }

            EnsureBufferCapacity(ref target, source.Count);
            source.CopyTo(target, 0);
            return source.Count;
        }


        #region == Properties ==

        [Category("Tracker Appearance")]
        [Browsable(true)]
        [DefaultValue(typeof(Color), "Black")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                if (_backbrush == null)
                {
                    _backbrush = new SolidBrush(value);
                }
                else
                {
                    _backbrush.Color = value;
                }                
            }
        }

        private bool _render = true;
        /// <summary>
        /// Set to true to enable rendering or false to pause rendering. Data is still refreshed when set to false.
        /// </summary>
        [Description("Set to true to enable rendering or false to pause rendering. Data is still refreshed when set to false.")]
        [Category("Tracker Behavior")]
        [Browsable(true)]
        [DefaultValue(true)]
        public bool Render
        {
            get
            {
                return _render;
            }
            set
            {
                _render = value;
            }
        }

        private Color _gridcolor = DefaultGridColor;
        /// <summary>
        /// The color of the grid.
        /// </summary>
        [Category("Tracker Grid")]
        [Description("The color of the grid.")]
        [Browsable(true)]
        public Color GridColor
        {
            get
            {
                return _gridcolor;
            }
            set
            {
                _gridcolor = value;
                if (_gridpen == null)
                {
                    _gridpen = new Pen(value, _gridthickness);
                    _gridpen.DashStyle = _griddashstyle;
                }
                else
                {
                    _gridpen.Color = value;
                }
            }
        }
        internal bool ShouldSerializeGridColor()
        {
            return _gridcolor != DefaultGridColor;
        }
        internal void ResetGridColor()
        {
            GridColor = DefaultGridColor;
        }

        private Color _gridthickercolor = DefaultGridThickerColor;
        /// <summary>
        /// The color of the thicker grid.
        /// </summary>
        [Category("Tracker Thicker Grid")]
        [Description("The color of the thicker grid.")]
        [Browsable(true)]
        public Color GridThickerColor
        {
            get
            {
                return _gridthickercolor;
            }
            set
            {
                _gridthickercolor = value;
                if (_gridthickerpen == null)
                {
                    _gridthickerpen = new Pen(value, _gridthickerthickness);
                    _gridthickerpen.DashStyle = _gridthickerdashstyle;
                }
                else
                {
                    _gridthickerpen.Color = value;
                }
            }
        }
        internal bool ShouldSerializeGridThickerColor()
        {
            return _gridthickercolor != DefaultGridThickerColor;
        }
        internal void ResetGridThickerColor()
        {
            GridThickerColor = DefaultGridThickerColor;
        }

        private float _gridthickness = 1;
        /// <summary>
        /// The thickness of the grid.
        /// </summary>
        [Category("Tracker Grid")]
        [Description("The thickness of the grid.")]
        [Browsable(true)]
        [DefaultValue(1)]
        public float GridThickness
        {
            get
            {
                return _gridthickness;
            }
            set
            {
                _gridthickness = value;
                if (_gridpen == null)
                {
                    _gridpen = new Pen(_gridcolor, value);
                    _gridpen.DashStyle = _griddashstyle;
                }
                else
                {
                    _gridpen.Width = value;
                }
            }
        }

        private float _gridthickerthickness = 1;
        /// <summary>
        /// The thickness of the thicker grid.
        /// </summary>
        [Category("Tracker Thicker Grid")]
        [Description("The thickness of the thicker grid.")]
        [Browsable(true)]
        [DefaultValue(1)]
        public float GridThickerThickness
        {
            get
            {
                return _gridthickerthickness;
            }
            set
            {
                _gridthickerthickness = value;
                if (_gridthickerpen == null)
                {
                    _gridthickerpen = new Pen(_gridthickercolor, value);
                    _gridthickerpen.DashStyle = _gridthickerdashstyle;
                }
                else
                {
                    _gridthickerpen.Width = value;
                }
            }
        }

        private System.Drawing.Drawing2D.DashStyle _griddashstyle = System.Drawing.Drawing2D.DashStyle.Dot;
        /// <summary>
        /// The dash style of the grid.
        /// </summary>
        [Category("Tracker Grid")]
        [Description("The dash style of the grid.")]
        [Browsable(true)]
        [DefaultValue(System.Drawing.Drawing2D.DashStyle.Dot)]
        public System.Drawing.Drawing2D.DashStyle GridDashStyle
        {
            get
            {
                return _griddashstyle;
            }
            set
            {
                _griddashstyle = value;
                if (_gridpen == null)
                {
                    _gridpen = new Pen(_gridcolor, _gridthickness);
                }
                _gridpen.DashStyle = value;
            }
        }

        private System.Drawing.Drawing2D.DashStyle _gridthickerdashstyle = System.Drawing.Drawing2D.DashStyle.Solid;
        /// <summary>
        /// The dash style of the thicker grid.
        /// </summary>
        [Category("Tracker Thicker Grid")]
        [Description("The dash style of the thicker grid.")]
        [Browsable(true)]
        [DefaultValue(System.Drawing.Drawing2D.DashStyle.Solid)]
        public System.Drawing.Drawing2D.DashStyle GridThickerDashStyle
        {
            get
            {
                return _gridthickerdashstyle;
            }
            set
            {
                _gridthickerdashstyle = value;
                if (_gridthickerpen == null)
                {
                    _gridthickerpen = new Pen(_gridthickercolor, _gridthickerthickness);
                }
                _gridthickerpen.DashStyle = value;
            }
        }

        private int _gridintervalx = 10;
        /// <summary>
        /// The interval between two vertical grid lines in pixels.
        /// </summary>
        [Category("Tracker Grid")]
        [Description("The interval between two vertical grid lines in pixels.")]
        [Browsable(true)]
        [DefaultValue(10)]
        public int GridIntervalX
        {
            get
            {
                return _gridintervalx;
            }
            set
            {
                _gridintervalx = value;
            }
        }

        private int _gridintervaly = 10;
        /// <summary>
        /// The interval between two horizontal grid lines in pixels.
        /// </summary>
        [Category("Tracker Grid")]
        [Description("The interval between two horizontal grid lines in pixels.")]
        [Browsable(true)]
        [DefaultValue(10)]
        public int GridIntervalY
        {
            get
            {
                return _gridintervaly;
            }
            set
            {
                _gridintervaly = value;
            }
        }

        private int _gridthickerevery = 5;
        /// <summary>
        /// Draw a thicker line every "n" grid lines where "n" is the value of this property.
        /// </summary>
        [Category("Tracker Thicker Grid")]
        [Description("Draw a thicker line every n grid lines where n is the value of this property.")]
        [Browsable(true)]
        [DefaultValue(5)]
        public int GridThickerEvery
        {
            get
            {
                return _gridthickerevery;
            }
            set
            {
                _gridthickerevery = value;
            }
        }

        private bool _griddrawthicker = true;
        /// <summary>
        /// Define wether thicker lines should be drawn or not every "n" grid lines where "n" is the value of GridThickerEvery.
        /// </summary>
        [Category("Tracker Thicker Grid")]
        [Description("Define wether thicker lines should be drawn or not every n grid lines where n is the value of GridThickerEvery.")]
        [Browsable(true)]
        [DefaultValue(true)]
        public bool GridDrawThicker
        {
            get
            {
                return _griddrawthicker;
            }
            set
            {
                _griddrawthicker = value;
            }
        }

        private bool _gridscroll = false;
        /// <summary>
        /// Define wether the grid should scroll or stay static. This has no effect if GridDrawThicker is set to false.
        /// </summary>
        [Category("Tracker Grid")]
        [Description("Define wether the grid should scroll or stay static. This has no effect if GridDrawThicker is set to false.")]
        [Browsable(true)]
        [DefaultValue(false)]
        public bool GridScroll
        {
            get
            {
                return _gridscroll;
            }
            set
            {
                _gridscroll = value;
                _gridoffsetx = 0;
            }
        }

        private int _maximum = 100;
        /// <summary>
        /// The maximum visible vertical value of the tracker.
        /// </summary>
        [Category("Tracker Data")]
        [Description("The maximum visible vertical value of the tracker.")]
        [Browsable(true)]
        [DefaultValue(100)]
        public int Maximum
        {
            get
            {
                return _maximum;
            }
            set
            {
                _maximum = value;
            }
        }

        private int _minimum = 0;
        /// <summary>
        /// The minimum visible vertical value of the tracker.
        /// </summary>
        [Category("Tracker Data")]
        [Description("The minimum visible vertical value of the tracker.")]
        [Browsable(true)]
        [DefaultValue(0)]
        public int Minimum
        {
            get
            {
                return _minimum;
            }
            set
            {
                _minimum = value;
            }
        }

        private int _maxdatainmemory = 100;
        /// <summary>
        /// The maximum amount of values each line feed should contain. Ideally, this value should be set depending on the width of the control. Do not set a too high value to preserve system resources.
        /// </summary>
        [Category("Tracker Data")]
        [Description("The maximum amount of values each line feed should contain. Ideally, this value should be set depending on the width of the control. Do not set a too high value to preserve system resources.")]
        [Browsable(true)]
        [DefaultValue(100)]
        public int MaxDataInMemory
        {
            get
            {
                return _maxdatainmemory;
            }
            set
            {
                _maxdatainmemory = value;
            }
        }

        private int _unitmargin = 4;
        /// <summary>
        /// A margin unit used in the control. Experiment different values to see which one fits you more.
        /// </summary>
        [Category("Tracker Appearance")]
        [Description("A margin unit used in the control. Experiment different values to see which one fits you more.")]
        [Browsable(true)]
        [DefaultValue(4)]
        public int UnitMargin
        {
            get
            {
                return _unitmargin;
            }
            set
            {
                _unitmargin = value;
            }
        }

        private bool _legendvisible = true;
        /// <summary>
        /// Define wether the legend should be shown or not.
        /// </summary>
        [Category("Tracker Legend")]
        [Description("Define wether the legend should be shown or not.")]
        [Browsable(true)]
        [DefaultValue(true)]
        public bool LegendVisible
        {
            get
            {
                return _legendvisible;
            }
            set
            {
                _legendvisible = value;
            }
        }

        private Byte _legendopacity = 128;
        /// <summary>
        /// The opacity of the legend background box.
        /// </summary>
        [Category("Tracker Legend")]
        [Description("The opacity of the legend background box.")]
        [Browsable(true)]
        [DefaultValue(128)]
        public Byte LegendOpacity
        {
            get
            {
                return _legendopacity;
            }
            set
            {
                _legendopacity = value;
            }
        }

        private int _refreshrate = 100;
        /// <summary>
        /// The refresh rate of the tracker in milliseconds. Do not set to a too small value to preserve system resources.
        /// </summary>
        [Category("Tracker Behavior")]
        [Description("The refresh rate of the tracker in milliseconds. Do not set to a too small value to preserve system resources.")]
        [Browsable(true)]
        [DefaultValue(100)]
        public int RefreshRate
        {
            get
            {
                return _refreshrate;
            }
            set
            {
                _refreshrate = value;
                if (internalTimer != null)
                {
                    internalTimer.Interval = value;
                }
            }
        }

        private bool _automax = false;
        /// <summary>
        /// If set to true, the Maximum property will be set automatically to fit the highest value in the tracker feeds.
        /// </summary>
        [Category("Tracker Behavior")]
        [Description("If set to true, the Maximum property will be set automatically to fit the highest value in the tracker feeds.")]
        [Browsable(true)]
        [DefaultValue(false)]
        public bool AutoMax
        {
            get
            {
                return _automax;
            }
            set
            {
                _automax = value;
            }
        }

        private double _automaxpercentage = 120;
        /// <summary>
        /// The percentage define how to compute the Maximum property according to the highest value in the tracker feeds. For example, if the highest value is 50 and the percentage is 120, the Maximum property will be set to 50 x 120% = 60.
        /// </summary>
        [Category("Tracker Behavior")]
        [Description("The percentage define how to compute the Maximum property according to the highest value in the tracker feeds. For example, if the highest value is 50 and the percentage is 120, the Maximum property will be set to 50 x 120% = 60.")]
        [Browsable(true)]
        [DefaultValue(120)]
        public double AutoMaxPercentage
        {
            get
            {
                return _automaxpercentage;
            }
            set
            {
                _automaxpercentage = value;
            }
        }

        private List<GabTrackerFeed> _feeds = new List<GabTrackerFeed>();
        /// <summary>
        /// The feeds of the tracker.
        /// </summary>
        [Category("Tracker Data")]
        [Description("The feeds of the tracker.")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)] /* VERY IMPORTANT ATTRIBUTE, AVOID SERIALIZATION */
        public List<GabTrackerFeed> Feeds
        {
            get
            {
                return _feeds;
            }
            set
            {
                _feeds = value;
            }
        }

        #endregion

/*        #region == Events ==

        // Declare the delegate (if using non-generic pattern).
        public delegate void TickEventHandler(object sender, EventArgs e);

        // Declare the event.
        public event TickEventHandler Tick;

        // Wrap the event in a protected virtual method
        // to enable derived classes to raise the event.
        protected virtual void onTick()
        {
            // Raise the event by using the () operator.
            Tick?.Invoke(this, new EventArgs());
        }

        #endregion*/

        /// <summary>
        /// Initialize a new instance of GabTracker.
        /// </summary>
        [Description("Initialize a new instance of GabTracker.")]
        public GabTracker()
        {
            InitializeComponent();

            //uncomment to find the resources full qualified names
            /*string[] sa = this.GetType().Assembly.GetManifestResourceNames();
            foreach (string s in sa)
                System.Diagnostics.Trace.WriteLine(s);*/

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            _gridpen = new Pen(Color.FromArgb(0,75,0));
            _gridpen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            _gridthickerpen = new Pen(Color.FromArgb(0, 75, 0));


            internalTimer.Start();
        }

        /// <summary>
        /// Repainting the control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void GabTracker_Paint(object sender, PaintEventArgs e)
        {
            double tmp1 = 0;
            double tmp2 = 0;
            double tmp3 = 0;
            Pen p;
            Pen fp;
            SolidBrush sb;
            string tmpstr;
            Color tmpcol;
            
            try
            {
                //background
                e.Graphics.FillRectangle(this._backbrush, e.ClipRectangle);

                //grid
                if (_griddrawthicker) //if we must draw the thicker grid then
                {
                    //vertical lines
                    for (int i = 0; i < e.ClipRectangle.Width; i = i + _gridintervalx)
                    {
                        tmp1 = _gridscroll ? i + (_gridoffsetx * _gridintervalx) : i;
                        if (tmp1 % (_gridthickerevery * _gridintervalx) == 0)
                        {
                            e.Graphics.DrawLine(this._gridthickerpen, i, 0, i, e.ClipRectangle.Height);
                        }
                        else
                        {
                            e.Graphics.DrawLine(this._gridpen, i, 0, i, e.ClipRectangle.Height);
                        }
                    }
                    //horizontal lines
                    for (int i = 0; i < e.ClipRectangle.Height; i = i + _gridintervaly)
                    {                        
                        if (i % (_gridthickerevery * _gridintervaly) == 0)
                        {
                            e.Graphics.DrawLine(this._gridthickerpen, 0, i, e.ClipRectangle.Width, i);
                        }
                        else
                        {
                            e.Graphics.DrawLine(this._gridpen, 0, i, e.ClipRectangle.Width, i);
                        }
                    }
                    if (_gridscroll) //if the grid must be scrolled then
                    {
                        _gridoffsetx++;
                        if (_gridoffsetx >= _gridthickerevery)
                        {
                            _gridoffsetx = 0;
                        }
                    }
                }
                else //we don't have to draw the thicker grid
                {
                    //vertical lines
                    for (int i = 0; i < e.ClipRectangle.Width; i = i + _gridintervalx)
                    {
                        e.Graphics.DrawLine(this._gridpen, i, 0, i, e.ClipRectangle.Height);
                    }
                    //horizontal lines
                    for (int i = 0; i < e.ClipRectangle.Height; i = i + _gridintervaly)
                    {
                        e.Graphics.DrawLine(this._gridpen, 0, i, e.ClipRectangle.Width, i);
                    }
                }

                //auto max
                if (_automax)
                {
                    double maxCandidate = _minimum;
                    foreach (GabTrackerFeed feed in _feeds)
                    {
                        if (feed.Data == null)
                        {
                            continue;
                        }

                        foreach (double value in feed.Data)
                        {
                            if (value > maxCandidate)
                            {
                                maxCandidate = value;
                            }
                        }
                    }

                    double computedMaximum = maxCandidate * (_automaxpercentage / 100d);
                    if (computedMaximum > _minimum)
                    {
                        _maximum = Convert.ToInt32(Math.Ceiling(computedMaximum));
                    }
                }

                double range = _maximum - _minimum;
                double valueScale = range != 0 ? (double)e.ClipRectangle.Height / range : 0d;

                //feed lines
                lock (_feeds)
                {
                    //for each feed in the tracker
                    foreach (GabTrackerFeed feed in _feeds)
                    {
                        int feedLength = CopyFeedData(feed.Data, ref _arrdata);
                        if (feedLength == 0)
                        {
                            continue;
                        }

                        p = feed.LinePen;
                        fp = feed.FillPen;
                        sb = new SolidBrush(feed.LineColor);
                        
                        //for each value in the feed data
                        for (int i = 0; i < feedLength; i++)
                        {
                            //if the feed is not inverted, do not invert the Y axis
                            if (!feed.Inverted)
                            {
                                tmp2 = (_maximum - _arrdata[i]) * valueScale;
                                if (i < feedLength - 1)
                                {
                                    tmp1 = (_maximum - _arrdata[i + 1]) * valueScale;
                                }
                                else
                                {
                                    tmp1 = tmp2;
                                }
                            }
                            else //we must invert the Y axis
                            {
                                tmp2 = _arrdata[i] * valueScale;
                                if (i < feedLength - 1)
                                {
                                    tmp1 = _arrdata[i + 1] * valueScale;
                                }
                                else
                                {
                                    tmp1 = tmp2;
                                }
                            }

                            //if we need to refresh the control
                            if (feed.NeedRefresh)
                            {
                                p.Dispose();
                                p = feed.LinePen;

                                fp.Dispose();
                                fp = feed.FillPen;

                                sb.Dispose();
                                sb = new SolidBrush(feed.LineColor);
                            }

                            //draw the line between the two current points of data
                                e.Graphics.DrawLine(p,
                                    e.ClipRectangle.Width - ((feedLength - (i+1)) * _gridintervalx),
                                    Convert.ToInt32(tmp1),
                                    e.ClipRectangle.Width - ((feedLength - (i+1)) * _gridintervalx + _gridintervalx),
                                    Convert.ToInt32(tmp2));

                            //we search for the lowest feed value superior to the current feed value
                            //in order to fill the line area.
                            tmp3 = e.ClipRectangle.Height;

                            if (feed.FillUnder) //if we must fill the space under the line then
                            {
                                if (!feed.FillSuperpose) //if we must superpose the fill then
                                {
                                    for (int k = 0; k < _feeds.Count; k++) //for each feed
                                    {
                                        if (_feeds[k].FillStopOtherFeeds) //if the feed must stop other feeds from filling
                                        {
                                            if (!_feeds[k].Equals(feed)) //if the feed is same reference than the current feed
                                            {
                                                int comparisonLength = CopyFeedData(_feeds[k].Data, ref _arrtmp);
                                                if (comparisonLength == 0)
                                                {
                                                    continue;
                                                }

                                                if (i < comparisonLength - 1)
                                                {
                                                    if (!_feeds[k].Inverted)
                                                    {
                                                        tmp2 = (_maximum - _arrtmp[i + 1]) * valueScale;
                                                    }
                                                    else
                                                    {
                                                        tmp2 = _arrtmp[i + 1] * valueScale;
                                                    }

                                                    if (tmp2 > tmp1)
                                                    {
                                                        if (tmp2 < tmp3)
                                                        {
                                                            tmp3 = tmp2 - Math.Ceiling(_feeds[k].LineThickness / 2);
                                                        }
                                                    }
                                                }
                                                else if (i < comparisonLength)
                                                {
                                                    if (!_feeds[k].Inverted)
                                                    {
                                                        tmp2 = (_maximum - _arrtmp[i]) * valueScale;
                                                    }
                                                    else
                                                    {
                                                        tmp2 = _arrtmp[i] * valueScale;
                                                    }

                                                    if (tmp2 > tmp1)
                                                    {
                                                        if (tmp2 < tmp3)
                                                        {
                                                            tmp3 = tmp2 - Math.Ceiling(_feeds[k].LineThickness / 2);
                                                        }
                                                    }
                                                }
                                                else if (i == comparisonLength)
                                                {
                                                    if (!_feeds[k].Inverted)
                                                    {
                                                        tmp2 = (_maximum - _arrtmp[i - 1]) * valueScale;
                                                    }
                                                    else
                                                    {
                                                        tmp2 = _arrtmp[i - 1] * valueScale;
                                                    }

                                                    if (tmp2 > tmp1)
                                                    {
                                                        if (tmp2 < tmp3)
                                                        {
                                                            tmp3 = tmp2 - Math.Ceiling(_feeds[k].LineThickness / 2);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    tmp3 = e.ClipRectangle.Height;
                                                }
                                            }
                                        }
                                    }
                                }

                                // draw the filling line under the data point
                                  e.Graphics.DrawLine(fp,
                                      e.ClipRectangle.Width - ((feedLength - (i+1)) * _gridintervalx),
                                      Convert.ToInt32(tmp1),
                                      e.ClipRectangle.Width - ((feedLength - (i+1)) * _gridintervalx),
                                      Convert.ToInt32(tmp3));
                            }

                        }

                        //generate the string for the line unit
                        tmpstr = feed.Value.ToString() + (feed.Unit == string.Empty ? String.Empty : " " + feed.Unit);
                        e.Graphics.DrawString(
                            tmpstr,
                            this.Font,
                            sb,
                            e.ClipRectangle.Width - (e.Graphics.MeasureString(tmpstr, this.Font).Width) - _unitmargin,
                            (float)tmp1 + _unitmargin,
                            StringFormat.GenericDefault);

                        //release resources
                        p.Dispose();
                        fp.Dispose();
                        sb.Dispose();
                    }

                    //if legend should be drawn then
                    if (_legendvisible)
                    {                        
                        tmp1 = 0;
                        tmp2 = 0;
                        tmpcol = Color.FromArgb(_legendopacity, this.BackColor);

                        //legend background fill calculations
                        for (int n = 0; n < _feeds.Count; n++)
                        {
                            tmpstr = _feeds[n].Legend == String.Empty ? "(not set)" : _feeds[n].Legend;
                            tmp3 = e.Graphics.MeasureString(tmpstr, this.Font).Width;
                            if (tmp3 > tmp1)
                            {
                                tmp1 = tmp3;
                            }
                            tmp3 = e.Graphics.MeasureString(tmpstr, this.Font).Height;
                            if (tmp3 > tmp2)
                            {
                                tmp2 = tmp3;
                            }
                        }

                        //legend background fill                        
                        using (SolidBrush bb = new SolidBrush(tmpcol))
                        {
                            e.Graphics.FillRectangle(
                                bb,
                                0,
                                Convert.ToInt32(e.ClipRectangle.Height - (_feeds.Count * tmp2) - _unitmargin),
                                Convert.ToInt32(tmp1) + (_unitmargin * 6),
                                Convert.ToInt32(_feeds.Count * tmp2) + _unitmargin);
                        }

                        //legend drawing
                        for (int n = 0; n < _feeds.Count; n++)
                        {
                            sb = new SolidBrush(_feeds[n].LineColor);
                            p = _feeds[n].LinePen;

                            tmpstr = _feeds[n].Legend == String.Empty ? "(not set)" : _feeds[n].Legend;
                            tmp3 = e.ClipRectangle.Height - (tmp2 * (n + 1) + (_unitmargin/2));

                            e.Graphics.DrawLine(
                                p,
                                _unitmargin,
                                Convert.ToInt32(tmp3 + tmp2),
                                _unitmargin * 4,
                                Convert.ToInt32(tmp3));

                            e.Graphics.DrawString(
                                tmpstr,
                                this.Font,
                                sb,
                                _unitmargin * 5,
                                Convert.ToInt32(tmp3),
                                StringFormat.GenericDefault);
                            sb.Dispose();
                            p.Dispose();
                        }
                    }
                }
                _haserror = false; //reset the error flag
            }
            catch (Exception ex)
            {
                if (!_haserror)
                {
                    _haserror = true;
                    MessageBox.Show(ex.ToString());
                }                
            }
        }

        /// <summary>
        /// Main timer procedure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void internalTimer_Tick(object sender, EventArgs e)
        {
            double value;

            //for each feed
            foreach (GabTrackerFeed feed in _feeds)
            {
                if (feed.Data == null)
                {
                    return;
                }

                //compute the new value
                value = feed.Value * feed.Coefficient;

                //add the new value in the queue
                feed.Data.Enqueue(value);
                
                //remove the superfluous data
                while (feed.Data.Count >= _maxdatainmemory)
                {
                    feed.Data.Dequeue();
                }
            }
            if (_render) //if Render is set to true, we force the control to repaint itself
            {
                this.Invalidate();
            }

/*            try
            {
                Tick?.Invoke(this, new EventArgs());
            }
            catch
            {
                
            }*/
           
        }

        /// <summary>
        /// Begin or resume data refreshing and rendering
        /// </summary>
        public void Begin()
        {
            internalTimer.Start();
        }

        /// <summary>
        /// Stops refreshing the control data and rendering
        /// </summary>
        public void Stop()
        {
            internalTimer.Stop();
        }
    }











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
        /// Retrieve the Pen used to fill under the line.
        /// </summary>
        [Browsable(false)]
        internal Pen FillPen
        {
            get
            {
                Pen p = new Pen(_fillcolor, _fillthickness);
                p.DashStyle = _filldashstyle;
                return p;
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
                _needrefresh = true;
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
                _needrefresh = true;
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
                _needrefresh = true;
            }
        }

        private System.Drawing.Drawing2D.DashStyle _linedashstyle = System.Drawing.Drawing2D.DashStyle.Solid;
        /// <summary>
        /// The dash style of the line.
        /// </summary>
        [Category("Feed Line")]
        [Description("The dash style of the line.")]
        [Browsable(true)]
        [DefaultValue(System.Drawing.Drawing2D.DashStyle.Solid)]
        public System.Drawing.Drawing2D.DashStyle LineDashStyle
        {
            get
            {
                return _linedashstyle;
            }
            set
            {
                _linedashstyle = value;
                _needrefresh = true;
            }
        }

        private System.Drawing.Drawing2D.DashStyle _filldashstyle = System.Drawing.Drawing2D.DashStyle.Solid;
        /// <summary>
        /// The dash style of the fill.
        /// </summary>
        [Category("Feed Fill")]
        [Description("The dash style of the fill.")]
        [Browsable(true)]
        [DefaultValue(System.Drawing.Drawing2D.DashStyle.Solid)]
        public System.Drawing.Drawing2D.DashStyle FillDashStyle
        {
            get
            {
                return _filldashstyle;
            }
            set
            {
                _filldashstyle = value;
                _needrefresh = true;
            }
        }

        private bool _needrefresh = false;
        /// <summary>
        /// Set to true to force the Tracker to refresh.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
        public bool NeedRefresh
        {
            get
            {
                if (_needrefresh)
                {
                    _needrefresh = false;
                    return true;
                }
                return false;
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
                _needrefresh = true;
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
        public GabTrackerFeed(Color color)
            : this()
        {
            LineColor = color;
        }

        /// <summary>
        /// Initializes a new instance of GabTrackerFeed.
        /// </summary>
        public GabTrackerFeed(Color color, float thickness)
            : this(color)
        {
            LineThickness = thickness;
        }

        /// <summary>
        /// Initializes a new instance of GabTrackerFeed.
        /// </summary>
        public GabTrackerFeed(Color color, float thickness, bool inverted)
            : this(color, thickness)
        {
            Inverted = inverted;
        }

        /// <summary>
        /// Initializes a new instance of GabTrackerFeed.
        /// </summary>
        public GabTrackerFeed(Color color, float thickness, bool inverted, double coefficient)
            : this(color, thickness, inverted)
        {
            Coefficient = coefficient;
        }

        /// <summary>
        /// Initializes a new instance of GabTrackerFeed.
        /// </summary>
        public GabTrackerFeed(Color color, float thickness, bool inverted, double coefficient, string unit)
            : this(color, thickness, inverted, coefficient)
        {
            Unit = unit;
        }

        /// <summary>
        /// Initializes a new instance of GabTrackerFeed.
        /// </summary>
        public GabTrackerFeed(Color color, float thickness, bool inverted, double coefficient, string unit, System.Drawing.Drawing2D.DashStyle dashstyle)
            : this(color, thickness, inverted, coefficient, unit)
        {
            LineDashStyle = dashstyle;
        }

        /// <summary>
        /// Initializes a new instance of GabTrackerFeed.
        /// </summary>
        public GabTrackerFeed(Color color, float thickness, bool inverted, double coefficient, string unit, System.Drawing.Drawing2D.DashStyle dashstyle, bool fillunder)
            : this(color, thickness, inverted, coefficient, unit, dashstyle)
        {
            FillUnder = fillunder;
        }

        /// <summary>
        /// Initializes a new instance of GabTrackerFeed.
        /// </summary>
        public GabTrackerFeed(Color color, float thickness, bool inverted, double coefficient, string unit, System.Drawing.Drawing2D.DashStyle dashstyle, bool fillunder, Byte fillalpha)
            : this(color, thickness, inverted, coefficient, unit, dashstyle, fillunder)
        {
            FillAlpha = fillalpha;
        }

        /// <summary>
        /// Initializes a new instance of GabTrackerFeed.
        /// </summary>
        public GabTrackerFeed(Color color, float thickness, bool inverted, double coefficient, string unit, System.Drawing.Drawing2D.DashStyle dashstyle, bool fillunder, Byte fillalpha, float fillthickness)
            : this(color, thickness, inverted, coefficient, unit, dashstyle, fillunder, fillalpha)
        {
            FillThickness = fillthickness;
        }

        /// <summary>
        /// Initializes a new instance of GabTrackerFeed.
        /// </summary>
        public GabTrackerFeed(Color color, float thickness, bool inverted, double coefficient, string unit, System.Drawing.Drawing2D.DashStyle dashstyle, bool fillunder, Byte fillalpha, float fillthickness, System.Drawing.Drawing2D.DashStyle filldashstyle)
            : this(color, thickness, inverted, coefficient, unit, dashstyle, fillunder, fillalpha, fillthickness)
        {
            FillDashStyle = filldashstyle;
        }

        /// <summary>
        /// Initializes a new instance of GabTrackerFeed.
        /// </summary>
        public GabTrackerFeed(Color color, float thickness, bool inverted, double coefficient, string unit, System.Drawing.Drawing2D.DashStyle dashstyle, bool fillunder, Byte fillalpha, float fillthickness, System.Drawing.Drawing2D.DashStyle filldashstyle, bool fillsuperpose)
            : this(color, thickness, inverted, coefficient, unit, dashstyle, fillunder, fillalpha, fillthickness, filldashstyle)
        {
            FillSuperpose = fillsuperpose;
        }

        /// <summary>
        /// Initializes a new instance of GabTrackerFeed.
        /// </summary>
        public GabTrackerFeed(Color color, float thickness, bool inverted, double coefficient, string unit, System.Drawing.Drawing2D.DashStyle dashstyle, bool fillunder, Byte fillalpha, float fillthickness, System.Drawing.Drawing2D.DashStyle filldashstyle, bool fillsuperpose, string legend)
            : this(color, thickness, inverted, coefficient, unit, dashstyle, fillunder, fillalpha, fillthickness, filldashstyle, fillsuperpose)
        {
            Legend = legend;
        }

        /// <summary>
        /// Initializes a new instance of GabTrackerFeed.
        /// </summary>
        public GabTrackerFeed(Color color, float thickness, bool inverted, double coefficient, string unit, System.Drawing.Drawing2D.DashStyle dashstyle, bool fillunder, Byte fillalpha, float fillthickness, System.Drawing.Drawing2D.DashStyle filldashstyle, bool fillsuperpose, string legend, bool fillstopotherfeeds)
            : this(color, thickness, inverted, coefficient, unit, dashstyle, fillunder, fillalpha, fillthickness, filldashstyle, fillsuperpose, legend)
        {
            FillStopOtherFeeds = fillstopotherfeeds;
        }

        /*protected GabTrackerFeed(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new System.ArgumentNullException("info");
            }
            _coefficient = double.Parse((string)info.GetValue("_coefficient", typeof(string)), CultureInfo.InvariantCulture);
            string strcol = (string)info.GetValue("_linecolor", typeof(string));
            string[] tabcol = strcol.Split(new Char[] {';'});
            _linecolor = Color.FromArgb(
                Int32.Parse(tabcol[0], CultureInfo.InvariantCulture),
                Int32.Parse(tabcol[1], CultureInfo.InvariantCulture),
                Int32.Parse(tabcol[2], CultureInfo.InvariantCulture),
                Int32.Parse(tabcol[3], CultureInfo.InvariantCulture));            
            _fillalpha = Byte.Parse((string)info.GetValue("_fillalpha", typeof(string)), CultureInfo.InvariantCulture);
            _fillcolor = Color.FromArgb(_fillalpha, _linecolor);
            _filldashstyle = (System.Drawing.Drawing2D.DashStyle)Int32.Parse((string)info.GetValue("_filldashstyle", typeof(string)), CultureInfo.InvariantCulture);
            _fillsuperpose = Boolean.Parse((string)info.GetValue("_fillsuperpose", typeof(string)));
            _fillthickness = float.Parse((string)info.GetValue("_fillthickness", typeof(string)), CultureInfo.InvariantCulture);
            _fillunder = Boolean.Parse((string)info.GetValue("_fillunder", typeof(string)));
            _inverted = Boolean.Parse((string)info.GetValue("_inverted", typeof(string)));
            _legend = (string)info.GetValue("_legend", typeof(string));
            _linedashstyle = (System.Drawing.Drawing2D.DashStyle)Int32.Parse((string)info.GetValue("_linedashstyle", typeof(string)), CultureInfo.InvariantCulture);
            _linethickness = float.Parse((string)info.GetValue("_linethickness", typeof(string)), CultureInfo.InvariantCulture);
            _unit = (string)info.GetValue("_unit", typeof(string));
            _value = double.Parse((string)info.GetValue("_value", typeof(string)), CultureInfo.InvariantCulture);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new System.ArgumentNullException("info");
            }
            info.AddValue("_coefficient", _coefficient.ToString(CultureInfo.InvariantCulture));
            info.AddValue("_linecolor",
                _linecolor.A.ToString(CultureInfo.InvariantCulture) + ";" +
                _linecolor.R.ToString(CultureInfo.InvariantCulture) + ";" +
                _linecolor.G.ToString(CultureInfo.InvariantCulture) + ";" + 
                _linecolor.B.ToString(CultureInfo.InvariantCulture));            
            info.AddValue("_fillalpha", _fillalpha.ToString(CultureInfo.InvariantCulture));
            info.AddValue("_filldashstyle", ((Int32)_filldashstyle).ToString(CultureInfo.InvariantCulture));
            info.AddValue("_fillsuperpose", _fillsuperpose.ToString());
            info.AddValue("_fillthickness", _fillthickness.ToString(CultureInfo.InvariantCulture));
            info.AddValue("_fillunder", _fillunder.ToString());
            info.AddValue("_inverted", _inverted.ToString());
            info.AddValue("_legend", _legend);
            info.AddValue("_linedashstyle", ((Int32)_linedashstyle).ToString(CultureInfo.InvariantCulture));
            info.AddValue("_linethickness", _linethickness.ToString(CultureInfo.InvariantCulture));
            info.AddValue("_unit", _unit);
            info.AddValue("_value", _value.ToString(CultureInfo.InvariantCulture));
        }*/
    }
}

