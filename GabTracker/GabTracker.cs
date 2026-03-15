using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        private readonly object _dataLock = new object();
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

        private double GetFeedY(GabTrackerFeed feed, double rawValue, double valueScale)
        {
            return feed.Inverted ? rawValue * valueScale : (_maximum - rawValue) * valueScale;
        }

        private double GetFeedYAtIndex(GabTrackerFeed feed, double[] buffer, int bufferLength, int dataIndex, double valueScale)
        {
            if (bufferLength == 0)
            {
                return 0d;
            }

            int clampedIndex = Math.Max(0, Math.Min(bufferLength - 1, dataIndex));
            return GetFeedY(feed, buffer[clampedIndex], valueScale);
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
        protected virtual void GabTracker_Paint(object sender, PaintEventArgs e)
        {
            double tmp1 = 0;
            double tmp2 = 0;
            double tmp3 = 0;
            Pen p;
            SolidBrush fb;
            SolidBrush sb;
            string tmpstr;
            Color tmpcol;
            
            try
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
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

                double range = _maximum - _minimum;
                double valueScale = range != 0 ? (double)e.ClipRectangle.Height / range : 0d;

                //feed lines
                lock (_dataLock)
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
                        fb = feed.FillBrush;
                        sb = feed.LineBrush;
                        
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

                            float segmentStartX = e.ClipRectangle.Width - ((feedLength - (i + 1)) * _gridintervalx);
                            float segmentEndX = segmentStartX - _gridintervalx;
                            float segmentStartY = (float)tmp1;
                            float segmentEndY = (float)tmp2;

                            //draw the line between the two current points of data
                            e.Graphics.DrawLine(
                                p,
                                segmentStartX,
                                segmentStartY,
                                segmentEndX,
                                segmentEndY);

                            if (feed.FillUnder)
                            {
                                double fillBottomStart = e.ClipRectangle.Height;
                                double fillBottomEnd = e.ClipRectangle.Height;
                                int startIndex = Math.Max(0, Math.Min(feedLength - 1, i + 1));
                                int endIndex = Math.Max(0, Math.Min(feedLength - 1, i));

                                if (!feed.FillSuperpose)
                                {
                                    for (int k = 0; k < _feeds.Count; k++)
                                    {
                                        if (!_feeds[k].FillStopOtherFeeds || _feeds[k].Equals(feed))
                                        {
                                            continue;
                                        }

                                        int comparisonLength = CopyFeedData(_feeds[k].Data, ref _arrtmp);
                                        if (comparisonLength == 0)
                                        {
                                            continue;
                                        }

                                        double otherStartY = GetFeedYAtIndex(_feeds[k], _arrtmp, comparisonLength, startIndex, valueScale);
                                        if (otherStartY > tmp1)
                                        {
                                            double candidate = otherStartY - Math.Ceiling(_feeds[k].LineThickness / 2);
                                            if (candidate < fillBottomStart)
                                            {
                                                fillBottomStart = candidate;
                                            }
                                        }

                                        double otherEndY = GetFeedYAtIndex(_feeds[k], _arrtmp, comparisonLength, endIndex, valueScale);
                                        if (otherEndY > tmp2)
                                        {
                                            double candidate = otherEndY - Math.Ceiling(_feeds[k].LineThickness / 2);
                                            if (candidate < fillBottomEnd)
                                            {
                                                fillBottomEnd = candidate;
                                            }
                                        }
                                    }
                                }

                                PointF[] fillPolygon = new[]
                                {
                                    new PointF(segmentStartX, (float)tmp1),
                                    new PointF(segmentEndX, (float)tmp2),
                                    new PointF(segmentEndX, (float)Math.Max(tmp2, fillBottomEnd)),
                                    new PointF(segmentStartX, (float)Math.Max(tmp1, fillBottomStart))
                                };

                                e.Graphics.FillPolygon(fb, fillPolygon);
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
                            sb = _feeds[n].LineBrush;
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

            lock (_dataLock)
            {
                //for each feed
                foreach (GabTrackerFeed feed in _feeds)
                {
                    if (feed.Data == null)
                    {
                        continue;
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

                        foreach (double v in feed.Data)
                        {
                            if (v > maxCandidate)
                            {
                                maxCandidate = v;
                            }
                        }
                    }

                    double computedMaximum = maxCandidate * (_automaxpercentage / 100d);
                    if (computedMaximum > _minimum)
                    {
                        _maximum = Convert.ToInt32(Math.Ceiling(computedMaximum));
                    }
                }
            }
            if (_gridscroll)
            {
                _gridoffsetx++;
                if (_gridoffsetx >= _gridthickerevery)
                {
                    _gridoffsetx = 0;
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
}
