﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using GoOutside.Events;
using GoOutside.Timers;

namespace GoOutside.ViewModels
{
    public class PomoViewModel : INotifyPropertyChanged, IPomoViewModel
    {
        private const string _Start = "Start";
        private const string _Cancel = "Cancel";

        private readonly IPomoTimer _PomoTimer;

        private bool _Visible;
        private string _TimerText;
        private Color _LightColour;
        private Color _DarkColour;
        private Color _BackgroundColour;

        private bool _ShowTimer = true;

        private readonly Color _LightWork = Color.FromArgb(255, 212, 0, 0);
        private readonly Color _DarkWork = Color.FromArgb(255, 128, 0, 0);
        private readonly Color _BackgroundWork = Color.FromArgb(255, 85, 0, 0);

        private readonly Color _LightRest = Color.FromArgb(255, 0, 212, 0);
        private readonly Color _DarkRest = Color.FromArgb(255, 0, 128, 0);
        private readonly Color _BackgroundRest = Color.FromArgb(255, 0, 85, 0);

        private readonly Color _LightDisabled = Color.FromArgb(255, 212, 212, 212);
        private readonly Color _DarkDisabled = Color.FromArgb(255, 128, 128, 128);
        private readonly Color _BackgroundDisabled = Color.FromArgb(255, 85, 85, 85);

        public event PropertyChangedEventHandler PropertyChanged;

        public double Height { get { return 200; } }
        public double Width { get { return 200; } }

        public Color LightColour
        {
            get { return _LightColour; }
            set
            {
                if (_LightColour == value) return;
                _LightColour = value;
                OnPropertyChanged();
            }
        }

        public Color DarkColour
        {
            get { return _DarkColour; }
            set
            {
                if (_DarkColour == value) return;
                _DarkColour = value;
                OnPropertyChanged();
            }
        }

        public Color BackgroundColour
        {
            get { return _BackgroundColour; }
            set
            {
                if (_BackgroundColour == value) return;
                _BackgroundColour = value;
                OnPropertyChanged();
            }
        }

        public bool Visible
        {
            get { return _Visible; }
            set
            {
                if (_Visible == value) return;
                _Visible = value;
                OnPropertyChanged();
            }
        }

        public string TimerText
        {
            get { return _TimerText; }
            set
            {
                if (_TimerText == value) return;
                _TimerText = value;
                OnPropertyChanged();
            }
        }

        public PomoViewModel(IPomoTimer pomoTimer)
        {
            _PomoTimer = pomoTimer;
            _PomoTimer.Tick += OnTick;
            _PomoTimer.StateChanged += OnStateChanged;
            Visible = true;
            TimerText = "25:00";
            LightColour = _LightDisabled;
            DarkColour = _DarkDisabled;
            BackgroundColour = _BackgroundDisabled;
        }

        public ICommand Show
        {
            get
            {
                return new DelegateCommand
                {
                    CanExecuteFunc = () => !Visible,
                    CommandAction = () =>
                    {
                        Visible = true;
                    }
                };
            }
        }

        public ICommand Hide
        {
            get
            {
                return new DelegateCommand
                {
                    CanExecuteFunc = () => Visible,
                    CommandAction = () =>
                    {
                        Visible = false;
                    }
                };
            }
        }

        public ICommand OnMouseEnter
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () =>
                    {
                        _ShowTimer = false;
                        TimerText = _PomoTimer.Running ? _Cancel : _Start;
                    }
                };
            }
        }

        public ICommand OnMouseLeave
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () =>
                    {
                        _ShowTimer = true;
                        if (!_PomoTimer.Running)
                        {
                            TimerText = "Start";
                        }
                    }
                };
            }
        }

        public ICommand OnMouseClick
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () =>
                    {
                        if (_PomoTimer.Running)
                        {
                            _PomoTimer.Stop();
                        }
                        else
                        {
                            _PomoTimer.Start();
                        }
                    }
                };
            }
        }

        private void OnTick(object sender, PomoTimerTickEventArgs args)
        {
            if (_ShowTimer) TimerText = args.TimeRemaining.ToString(@"mm\:ss");
        }

        private void OnStateChanged(object sender, PomoTimerStateChangeEventArgs eventargs)
        {
            switch (eventargs.State)
            {
                case PomoTimerState.Disabled:
                    Disabled();
                    break;
                case PomoTimerState.Work:
                    Work();
                    break;
                case PomoTimerState.Rest:
                    Rest();
                    break;
            }
        }

        private void Work()
        {
            LightColour = _LightWork;
            DarkColour = _DarkWork;
            BackgroundColour = _BackgroundWork;
        }

        private void Rest()
        {
            LightColour = _LightRest;
            DarkColour = _DarkRest;
            BackgroundColour = _BackgroundRest;
        }

        private void Disabled()
        {
            LightColour = _LightDisabled;
            DarkColour = _DarkDisabled;
            BackgroundColour = _BackgroundDisabled;
        }

        private void OnPropertyChanged([CallerMemberName] string caller = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}