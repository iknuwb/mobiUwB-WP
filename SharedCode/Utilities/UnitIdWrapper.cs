using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedCode.Utilities
{
    /// <summary>
    /// Opakowywacz ID jednostki.
    /// </summary>
    public class UnitIdWrapper
    {
        private String _unitId;
        /// <summary>
        /// Pobiera lub nadaje ID jednostki.
        /// </summary>
        public String UnitId
        {
            get { return _unitId; }
            set { _unitId = value; }
        }
    }
}
