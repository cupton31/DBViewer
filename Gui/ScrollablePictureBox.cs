using System;
using System.Drawing;
using System.Windows.Forms;

using System.ComponentModel;

namespace DBViewer.Gui
{
	/// <summary>
	/// ScrollablePictureBox - extends the PictureBox control to include scroll bars.
	/// </summary>
	public class ScrollablePictureBox : PictureBox
	{

		#region Private Constants.
		private const float Resolution = 144.0f;
		private const double JpgDistortion = 0.667;
		private const int LargeScrollStep = 10;
		private const int SmallScrollStep = 20;
		#endregion Private Constants.

		#region Private Members.
		/// <summary>
		/// Horizontal offset of the image when scrolled.
		/// </summary>
		private int offsetX;
		/// <summary>
		/// Vertical offset of the image when scrolled.
		/// </summary>
		private int offsetY;
		/// <summary>
		/// Vertical scroll bar.
		/// </summary>
		private HScrollBar hScrollBar;
		/// <summary>
		/// Horizontal scroll bar.
		/// </summary>
		private VScrollBar vScrollBar;
		/// <summary>
		/// The type of the image beeing loaded.
		/// </summary>
		private ImageType imageType;
		#endregion Private Members.

		#region Private Methods.
		/// <summary>
		/// HScrollBarScrollEventHandler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void HScrollBarScrollEventHandler(object sender, ScrollEventArgs e)
		{
			OffsetX = e.NewValue;
		}
		/// <summary>
		/// VScrollBarScrollEventHandler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void VScrollBarScrollEventHandler(object sender, ScrollEventArgs e)
		{
			OffsetY = e.NewValue;
		}
		/// <summary>
		/// Sets image type according to the HorizontalResolution & VerticalResolution.
		/// </summary>
		/// <param name="image">image</param>
		private void SetImageType(Image image)
		{
			if(imageType != ImageType.None)
				return;
			if(image.HorizontalResolution == Resolution && image.VerticalResolution == Resolution)
				imageType = ImageType.Jpg;
			else
				imageType = ImageType.Bmp;
		}
		#endregion Private Methods.

		#region Protected Overrided Methods.
		/// <summary>
		/// Paints the image in the PictureBox.
		/// </summary>
		/// <param name="e">PaintEvent arguments.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);
			if(Image != null)
			{
				Graphics g = e.Graphics;
				g.FillRectangle(Brushes.White,this.ClientRectangle);
				g.DrawImageUnscaled(Image, -OffsetX, -OffsetY, Image.Width, Image.Height);
			}
		
		}
		/// <summary>
		/// Updates the scroll bars when resizing.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(System.EventArgs e)
		{
			base.OnResize (e);
            RefreshPictureBox();			
		}

		#endregion Protected Overrided Methods.

		#region Public Properties.
		/// <summary>
		/// Horizontal offset of the image when scrolled.
		/// </summary>
		[Category("Scroll Offset")]
		[Description("Horizontal offset of the image when scrolled.")]
		public int OffsetX
		{
			get
			{
				return offsetX;
			}
			set
			{
				offsetX = value;
				this.Invalidate();
			}
		}

		/// <summary>
		/// Vertical offset of the image when scrolled.
		/// </summary>
		[Category("Scroll Offset")]
		[Description("Vertical offset of the image when scrolled.")]
		public int OffsetY
		{
			get
			{
				return offsetY;
			}
			set
			{
				offsetY = value;
				this.Invalidate();
			}
		}
		/// <summary>
		/// Gets or set the lower limit of values of the horizontal scrollable range
		/// </summary>
		[Category("Horizontal ScrollBar")]
		[Description("Gets or sets the lower limit of values of the horizontal scrollable range")]
		public int HorizontalMinimum
		{
			get
			{
				return hScrollBar.Minimum;
			}
			set
			{
				hScrollBar.Minimum = value;
			}
		}

		/// <summary>
		/// Gets or sets the upper limit of values of the horizontal scrollable range
		/// </summary>
		[Category("Horizontal ScrollBar")]
		[Description("Gets or sets the upper limit of values of the horizontal scrollable range")]
		public int HorizontalMaximum
		{
			get
			{
				return hScrollBar.Maximum;
			}
			set
			{
				hScrollBar.Maximum = value;
			}
		}
		/// <summary>
		/// Gets or sets a value to be added to or subtracted from the Value property when the horizontal scroll box is moved a large distance
		/// </summary>
		[Category("Horizontal ScrollBar")]
		[Description("Gets or sets a value to be added to or subtracted from the Value property when the horizontal scroll box is moved a large distance")]
		public int HorizontalLargeChange
		{
			get
			{
				return hScrollBar.LargeChange;
			}
			set
			{
				hScrollBar.LargeChange = value;
			}
		}
		/// <summary>
		/// Gets or sets the value to be added to or subtracted from the Value property when the horizontal scroll box is moved a small distance
		/// </summary>
		[Category("Horizontal ScrollBar")]
		[Description("Gets or sets the value to be added to or subtracted from the Value property when the horizontal scroll box is moved a small distance")]
		public int HorizontalSmallChange
		{
			get
			{
				return hScrollBar.SmallChange;
			}
			set
			{
				hScrollBar.SmallChange = value;
			}
		}
		/// <summary>
		/// Gets or sets a numeric value that represents the current position of the scroll box on the horizontal scroll bar control.
		/// </summary>
		[Category("Horizontal ScrollBar")]
		[Description("Gets or sets a numeric value that represents the current position of the scroll box on the horizontal scroll bar control")]
		public int HorizontalValue
		{
			get
			{
				return hScrollBar.Value;
			}
			set
			{
				hScrollBar.Value = value;
			}
		}
		/// <summary>
		/// Gets or set the lower limit of values of the vertical scrollable range
		/// </summary>
		[Category("Vertical ScrollBar")]
		[Description("Gets or sets the lower limit of values of the vertical scrollable range")]
		public int VerticalMinimum
		{
			get
			{
				return vScrollBar.Minimum;
			}
			set
			{
				vScrollBar.Minimum = value;
			}
		}
		/// <summary>
		/// Gets or sets the upper limit of values of the vertical scrollable range
		/// </summary>
		[Category("Vertical ScrollBar")]
		[Description("Gets or sets the upper limit of values of the vertical scrollable range")]
		public int VerticalMaximum
		{
			get
			{
				return vScrollBar.Maximum;
			}
			set
			{
				vScrollBar.Maximum = value;
			}
		}
		/// <summary>
		/// Gets or sets a value to be added to or subtracted from the Value property when the vertical scroll box is moved a large distance
		/// </summary>
		[Category("Vertical ScrollBar")]
		[Description("Gets or sets a value to be added to or subtracted from the Value property when the vertical scroll box is moved a large distance")]
		public int VerticalLargeChange
		{
			get
			{
				return vScrollBar.LargeChange;
			}
			set
			{
				vScrollBar.LargeChange = value;
			}
		}
		/// <summary>
		/// Gets or sets the value to be added to or subtracted from the Value property when the vertical scroll box is moved a small distance
		/// </summary>
		[Category("Vertical ScrollBar")]
		[Description("Gets or sets the value to be added to or subtracted from the Value property when the vertical scroll box is moved a small distance")]
		public int VerticalSmallChange
		{
			get
			{
				return vScrollBar.SmallChange;
			}
			set
			{
				vScrollBar.SmallChange = value;
			}
		}
		/// <summary>
		/// Gets or sets a numeric value that represents the current position of the scroll box on the vertical scroll bar control
		/// </summary>
		[Category("Vertical ScrollBar")]
		[Description("Gets or sets a numeric value that represents the current position of the scroll box on the vertical scroll bar control")]
		public int VerticalValue
		{
			get
			{
				return vScrollBar.Value;
			}
			set
			{
				vScrollBar.Value = value;
			}
		}
		#endregion Public Properties.

		#region Public Default Constructor.
		/// <summary>
		/// Constructor.
		/// </summary>
		public ScrollablePictureBox() : base()
		{
			hScrollBar = new HScrollBar();
			hScrollBar.Dock = DockStyle.Bottom;
			hScrollBar.Scroll += new ScrollEventHandler(HScrollBarScrollEventHandler);

			vScrollBar = new VScrollBar();
			vScrollBar.Dock = DockStyle.Right;
			vScrollBar.Scroll += new ScrollEventHandler(VScrollBarScrollEventHandler);

			this.Controls.Add(hScrollBar);
			this.Controls.Add(vScrollBar);

			OffsetX = 0;
			OffsetY = 0;

		}
		#endregion Public Default Constructor.		

		#region Public Methods.
		/// <summary>
		/// Refreshes the Pcture'sBox image
		/// </summary>
		public void RefreshPictureBox()
		{
            if(Image != null)
			{
				this.SuspendLayout();
				this.Visible = false;
				
				OffsetX = 0;
				OffsetY = 0;
				HorizontalValue = 0;
				VerticalValue = 0;

				SetImageType(Image);

				switch(imageType)
				{
					case ImageType.None:
					{
						throw new ApplicationException("Invalid image type.");
					}
					case ImageType.Jpg:
					{
						HorizontalMaximum = (int) (Image.Width * JpgDistortion - this.ClientSize.Width  + vScrollBar.Width);
						VerticalMaximum = (int) (Image.Height * JpgDistortion- this.ClientSize.Height + hScrollBar.Height);	
						break;
					}					
					case ImageType.Bmp:
					{
						HorizontalMaximum = Image.Width - this.ClientSize.Width  + vScrollBar.Width;
						VerticalMaximum = Image.Height - this.ClientSize.Height + hScrollBar.Height;
						break;
					}

				}

				HorizontalLargeChange = HorizontalMaximum / LargeScrollStep;
				VerticalLargeChange = VerticalMaximum / LargeScrollStep;
				

				HorizontalSmallChange = HorizontalMaximum / SmallScrollStep;
				VerticalSmallChange = VerticalMaximum / SmallScrollStep;

				this.Visible = true;
				this.ResumeLayout();	
			}
		}
		#endregion Public Methods.
	}
}
