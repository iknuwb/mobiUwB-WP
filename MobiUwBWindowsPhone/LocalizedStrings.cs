#region

using MobiUwB.Resources;

#endregion

namespace MobiUwB
{
    /// <summary>
    /// Dostarcza dostęp do StringResource's.
    /// </summary>
    public class LocalizedStrings
    {
        /// <summary>
        /// Obiekt StringResource's.
        /// </summary>
        private static readonly AppResources _localizedResources = new AppResources();

        /// <summary>
        /// Pobiera StringResource's.
        /// </summary>
        public AppResources LocalizedResources
        {
            get { return _localizedResources; }
        }
    }
}