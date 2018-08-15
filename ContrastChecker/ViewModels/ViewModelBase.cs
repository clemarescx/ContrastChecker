using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ContrastChecker.Annotations;

namespace ContrastChecker.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        [NotifyPropertyChangedInvocator]
        protected virtual bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;

            NotifyPropertyChanged(propertyName);
            return true;
        }

        [NotifyPropertyChangedInvocator]
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
