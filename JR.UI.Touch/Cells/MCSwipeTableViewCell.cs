using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Drawing;
using MonoTouch.ObjCRuntime;
using Cirrious.MvvmCross.Binding.Touch.Views;



namespace JR.UI.Touch
{

	
	public enum MCSwipeTableViewCellState{
		MCSwipeTableViewCellStateNone = 0,
		MCSwipeTableViewCellState1,
		MCSwipeTableViewCellState2,
		MCSwipeTableViewCellState3,
		MCSwipeTableViewCellState4
	}

	public enum MCSwipeTableViewCellMode 
	{ 
		MCSwipeTableViewCellModeExit = 0, 
		MCSwipeTableViewCellModeSwitch
	}

	public enum MCSwipeTableViewCellDirection{
		MCSwipeTableViewCellDirectionLeft = 0,
		MCSwipeTableViewCellDirectionCenter,
		MCSwipeTableViewCellDirectionRight
	}


	public class MCSwipeTableViewCell : MvxStandardTableViewCell
	{
		public static NSString Identifier = new NSString("MCSwipeTableViewCell");

		public const string BindingText = @"
											SpeakerText Item.Advert.SpeakerKey;
											MainText Item.Advert.Title;
											RoomText Item.Advert,Converter=AdvertSmallDetails,ConverterParameter='SmallDetailsFormat';
											SelectedCommand Command;
											IsFavorite Item.IsFavorite
											";
		public string _firstIconName;
		public string _secondIconName;
		public string _thirdIconName;
		public string _fourthIconName;
		public string _currentImageName;
		public float _currentPercentage;
		public UIColor _firstColor;
		public UIColor _secondColor;
		public UIColor _thirdColor;
		public UIColor _fourthColor;
		public MCSwipeTableViewCellMode _mode;
		public UIView _colorIndicatorView;
		public UIPanGestureRecognizer _panGestureRecognizer;
		public UIImageView _slidingImageView;
		public MCSwipeTableViewCellDirection _direction;

		public static float kMCStop1 = 0.20F; // Percentage limit to trigger the first action
		public static float kMCStop2 = 0.75F; // Percentage limit to trigger the second action
		public static float kMCBounceAmplitude = 20.0F; // Maximum bounce amplitude when using the MCSwipeTableViewCellModeSwitch mode
		public static TimeSpan kMCBounceDuration2 = new TimeSpan((long)0.1); // Duration of the second part of the bounce animation
		public static TimeSpan kMCDurationLowLimit = new TimeSpan((long)0.25); // Lowest duration when swipping the cell because we try to simulate velocity
		public static TimeSpan kMCDurationHightLimit = new TimeSpan((long)0.1); // Highest duration when swipping the cell because we try to simulate velocity

		public static MCSwipeTableViewCell LoadFromNib(NSObject owner)
		{
			// this bizarre loading sequence is modified from a blog post on AlexYork.net
			// basically we create an empty cell in C#, then pass that through a NIB loading, which then magically
			// gives us a new cell back in MonoTouch again
			var views = NSBundle.MainBundle.LoadNib("MCSwipeTableViewCell", owner, null);
			var cell2 = Runtime.GetNSObject( views.ValueAt(0) ) as MCSwipeTableViewCell;

			views = null;
			cell2.Initialise();
			return cell2;
		}


		public MCSwipeTableViewCell(IntPtr handle)
			: base(BindingText, handle)
		{
		}		

		public MCSwipeTableViewCell ()
			: base(BindingText, MonoTouch.UIKit.UITableViewCellStyle.Default, Identifier)
		{
		}

		public MCSwipeTableViewCell (string bindingText)
			: base(bindingText, MonoTouch.UIKit.UITableViewCellStyle.Default, Identifier)
		{
		}



		private void Initialise()
		{
			_mode = MCSwipeTableViewCellMode.MCSwipeTableViewCellModeSwitch;

			_colorIndicatorView = new UIView (this.Bounds);
			 
			_colorIndicatorView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
			_colorIndicatorView.BackgroundColor = UIColor.Clear;
			this.InsertSubviewBelow (this.ContentView, _colorIndicatorView);
			_slidingImageView = new UIImageView ();
			_slidingImageView.ContentMode = UIViewContentMode.Center;



			_panGestureRecognizer = new UIPanGestureRecognizer();
			_panGestureRecognizer.AddTarget ( () => { HandlePanGestureRecognizer (_panGestureRecognizer); });
			this.AddGestureRecognizer (_panGestureRecognizer);
			//_panGestureRecognizer.Delegate = this;
		}
		
		public void HandlePanGestureRecognizer(UIPanGestureRecognizer gesture)
		{
			UIGestureRecognizerState state = gesture.State;
		
			System.Drawing.PointF translation = gesture.TranslationInView(this);
			System.Drawing.PointF velocity = gesture.VelocityInView(this);
			float percentage = this.PercentageWithOffset(this.ContentView.Frame.X, this.Bounds.Width);
			TimeSpan animationDuration =  AnimationDurationWithVelocity(velocity);
			_direction = this.DirectionWithPercentage(percentage);

			if (state == UIGestureRecognizerState.Began) {
			}
			else if (state == UIGestureRecognizerState.Began || state == UIGestureRecognizerState.Changed) {
				System.Drawing.PointF center = new System.Drawing.PointF(this.ContentView.Center.X + translation.X, this.ContentView.Center.Y);
				this.ContentView.Center = center;
				this.AnimateWithOffset (this.ContentView.Frame.X);
				gesture.SetTranslation(PointF.Empty,this);
			}
			else if (state == UIGestureRecognizerState.Ended || state == UIGestureRecognizerState.Cancelled) {
				_currentImageName = this.ImageNameWithPercentage(percentage);
				_currentPercentage = percentage;
				MCSwipeTableViewCellState cellState= this.StateWithPercentage(percentage);

				if (_mode == MCSwipeTableViewCellMode.MCSwipeTableViewCellModeExit && _direction != MCSwipeTableViewCellDirection.MCSwipeTableViewCellDirectionCenter && this.ValidateState(cellState))
					this.MoveWithDuration(animationDuration, _direction);
				else
					this.BounceToOrigin();
			}
		}



		public float PercentageWithOffset (float offset,  float width)
		{
			float percentage = offset / width;

			if (percentage < -1.0F) percentage = -1.0F;
			else if (percentage > 1.0F) percentage = 1.0F;

			return percentage;
		}

		
		public MCSwipeTableViewCellDirection DirectionWithPercentage(float percentage) 
		{
			if (percentage < -kMCStop1)
				return MCSwipeTableViewCellDirection.MCSwipeTableViewCellDirectionLeft;
			else if (percentage > kMCStop1)
				return MCSwipeTableViewCellDirection.MCSwipeTableViewCellDirectionRight;
			else
				return MCSwipeTableViewCellDirection.MCSwipeTableViewCellDirectionCenter;
		}
		
		public TimeSpan AnimationDurationWithVelocity(System.Drawing.PointF velocity) 
		{
			float width = this.Bounds.Width;
			TimeSpan animationDurationDiff = kMCDurationHightLimit - kMCDurationLowLimit;
			float horizontalVelocity = velocity.X;

			if (horizontalVelocity < -width) horizontalVelocity = -width;
			else if (horizontalVelocity > width) horizontalVelocity = width;

			return (kMCDurationHightLimit + kMCDurationLowLimit); // TODO FIX - Math.Abs(((horizontalVelocity / width) * animationDurationDiff)));
		}
		
		public void AnimateWithOffset(float offset) 
		{
			float percentage =  this.PercentageWithOffset(offset,this.Bounds.Width);

			// Image Name
			string imageName = this.ImageNameWithPercentage(percentage);

			// Image Position
			if (imageName != null) {

				//TODO FIX
				//[_slidingImageView setImage:[UIImage imageNamed:imageName]];
				//[_slidingImageView setAlpha:[self imageAlphaWithPercentage:percentage]];

			}

			this.SlideImageWithPercentage(percentage,imageName,true);

			// Color
			UIColor color = this.ColorWithPercentage(percentage);
			if (color != null) {
				_colorIndicatorView.BackgroundColor = color;
			}
		}
		
		public string ImageNameWithPercentage (float percentage) 
		{
			string imageName = null;

			// Image
			if (percentage >= 0 && percentage < kMCStop2)
				imageName = _firstIconName;
			else if (percentage >= kMCStop2)
				imageName = _secondIconName;
			else if (percentage < 0 && percentage > -kMCStop2)
				imageName = _thirdIconName;
			else if (percentage <= -kMCStop2)
				imageName = _fourthIconName;

			return imageName;
		}

		
		public UIColor ColorWithPercentage (float percentage) 
		{
			UIColor color;

			// Background Color
			if (percentage >= kMCStop1 && percentage < kMCStop2)
				color = _firstColor;
			else if (percentage >= kMCStop2)
				color = _secondColor;
			else if (percentage < -kMCStop1 && percentage > -kMCStop2)
				color = _thirdColor;
			else if (percentage <= -kMCStop2)
				color = _fourthColor;
			else
				color = UIColor.Clear;

			return color;
		}

		

		public void SlideImageWithPercentage ( float percentage, string imageName, bool isDragging) 
		{
			UIImage slidingImage = new UIImage(imageName); //TODO FIX ME
			System.Drawing.SizeF slidingImageSize = slidingImage.Size;

			//ARRIVATO QUI
			RectangleF slidingImageRect;

			PointF position = PointF.Empty;

			position.Y= this.Bounds.Height / 2.0F;

			if (isDragging) {
				if (percentage >= 0 && percentage < kMCStop1) {
					position.X = this.OffsetWithPercentage((kMCStop1 / 2), this.Bounds.Width);
				}

				else if (percentage >= kMCStop1) {
					position.X = this.OffsetWithPercentage(percentage - (kMCStop1 / 2), this.Bounds.Width);
				}
				else if (percentage < 0 && percentage >= -kMCStop1) {
					position.X = this.Bounds.Width - this.OffsetWithPercentage( (kMCStop1 / 2),this.Bounds.Width);
				}

				else if (percentage < -kMCStop1) {
					position.X = this.Bounds.Width + this.OffsetWithPercentage( percentage + (kMCStop1 / 2),this.Bounds.Width);
				}
			}
			else {
				if (_direction == MCSwipeTableViewCellDirection.MCSwipeTableViewCellDirectionRight) {
					position.X = this.OffsetWithPercentage(percentage - (kMCStop1 / 2), this.Bounds.Width);
				}
				else if (_direction == MCSwipeTableViewCellDirection.MCSwipeTableViewCellDirectionLeft) {
					position.X = this.Bounds.Width + this.OffsetWithPercentage( percentage + (kMCStop1 / 2),this.Bounds.Width);
				}
				else {
					return;
				}
			}


			slidingImageRect = new RectangleF(position.X - slidingImageSize.Width / 2.0F,
			                              position.Y - slidingImageSize.Height / 2.0F,
			                              slidingImageSize.Width,
			                              slidingImageSize.Height);

			//slidingImageRect = CGRectIntegral(slidingImageRect); //TODO FIX
			_slidingImageView.Frame = slidingImageRect;
		}

		
		public float OffsetWithPercentage (float percentage, float width) 
		{
			float offset = percentage * width;

			if (offset < -width) offset = -width;
			else if (offset > width) offset = width;

			return offset;
		}

	 	public MCSwipeTableViewCellState StateWithPercentage (float percentage) 
		{
			MCSwipeTableViewCellState state;

			state = MCSwipeTableViewCellState.MCSwipeTableViewCellStateNone;

			if (percentage >= kMCStop1 && this.ValidateState(MCSwipeTableViewCellState.MCSwipeTableViewCellState1))
			    state = MCSwipeTableViewCellState.MCSwipeTableViewCellState1;

			    if (percentage >= kMCStop2 && this.ValidateState(MCSwipeTableViewCellState.MCSwipeTableViewCellState2))
			    state = MCSwipeTableViewCellState.MCSwipeTableViewCellState2;

			    if (percentage <= -kMCStop1 && this.ValidateState(MCSwipeTableViewCellState.MCSwipeTableViewCellState3))
			    state = MCSwipeTableViewCellState.MCSwipeTableViewCellState3;

			    if (percentage <= -kMCStop2 && this.ValidateState(MCSwipeTableViewCellState.MCSwipeTableViewCellState4))
			    state = MCSwipeTableViewCellState.MCSwipeTableViewCellState4;

			return state;
		}

		public bool ValidateState (MCSwipeTableViewCellState state) 
		{
			bool isValid = true;

			switch (state) {
			case MCSwipeTableViewCellState.MCSwipeTableViewCellStateNone: 
				isValid = false;
			
				break;

			case MCSwipeTableViewCellState.MCSwipeTableViewCellState1: 
				//if (!_firstColor && !_firstIconName) //TODO FIX
					//isValid = false;
			
				break;

			case MCSwipeTableViewCellState.MCSwipeTableViewCellState2: 
				//if (!_secondColor && !_secondIconName)
					//isValid = false;
			
				break;

			case MCSwipeTableViewCellState.MCSwipeTableViewCellState3: 
				//if (!_thirdColor && !_thirdIconName)
					//isValid = false;
			
				break;

			case MCSwipeTableViewCellState.MCSwipeTableViewCellState4:
				//if (!_fourthColor && !_fourthIconName)
					//isValid = false;
			
				break;

				default:
				break;
			}

			return isValid;
		}

			

		public void MoveWithDuration(TimeSpan duration , MCSwipeTableViewCellDirection direction) 
		{
				float origin;

			if(direction == MCSwipeTableViewCellDirection.MCSwipeTableViewCellDirectionLeft)
					origin = -this.Bounds.Width;
				else
					origin = this.Bounds.Width;

				float percentage = this.PercentageWithOffset(origin, this.Bounds.Width);
				RectangleF rect = this.ContentView.Frame;
				rect.Location.X = origin;

				// Color
				UIColor color = this.ColorWithPercentage(_currentPercentage);
				if (color != null) {
					_colorIndicatorView.BackgroundColor = color;
				}

				// Image
				if (_currentImageName != null) {
					_slidingImageView.Image= new UIImage(_currentImageName);
				}
				/*
				UIView.AnimateWithDuration(duration,0.0,
				                          (UIViewAnimationOptionCurveEaseOut | UIViewAnimationOptionAllowUserInteraction),
				 animations:^{
					[self.contentView setFrame:rect];
					[_slidingImageView setAlpha:0];
					[self slideImageWithPercentage:percentage imageName:_currentImageName isDragging:NO];
				 }
				 completion:^(BOOL finished) {
					[self notifyDelegate];
				 }];
				 */ //TODO FIX
	}

			
		public void BounceToOrigin(){
				float bounceDistance = kMCBounceAmplitude * _currentPercentage;
				/*
				[UIView animateWithDuration:kMCBounceDuration1
				 delay:0
				 options:(UIViewAnimationOptionCurveEaseOut)
				 animations:^{
					CGRect frame = self.contentView.frame;
					frame.origin.x = -bounceDistance;
					[self.contentView setFrame:frame];
					[_slidingImageView setAlpha:0.0];
					[self slideImageWithPercentage:0 imageName:_currentImageName isDragging:NO];
				 }
				 completion:^(BOOL finished1) {

					[UIView animateWithDuration:kMCBounceDuration2
					 delay:0
					 options:UIViewAnimationOptionCurveEaseIn
					 animations:^{
						CGRect frame = self.contentView.frame;
						frame.origin.x = 0;
						[self.contentView setFrame:frame];
					 }
					 completion:^(BOOL finished2) {
						[self notifyDelegate];
					 }];
				 }];
			}
*/ //TODO FIX







	}
	/*
	public class MCSwipeTableViewCellDelegate : Object
	{
	}
	*/
	}
}

