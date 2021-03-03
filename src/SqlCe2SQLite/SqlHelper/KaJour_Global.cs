using System;
using System.Collections.Generic;

//
// Public / Global
//

namespace KaJourDAL
{
    public static class KaJour_Global
    {
        // Instance-Variables
        private static string m_globalVar = "";
        private static string m_sys_prog = "";
        //public static string SQLProvider;

        // Properties
        public static string GlobalVar
        {
            get { return m_globalVar; }
            set { m_globalVar = value; }
        }
        
        public static string sys_prog
        {
            get { return m_sys_prog; }
            set { m_sys_prog = value; }
        }
        public static string sys_rzmand { get; set; }
        /// <summary>
        /// v:1.0.20200101.1730
        /// </summary>
        public static string sys_ver { get; set; }
        public static string prg_back { get; set; }
        public static string stxt_auth { get; set; }
        public static bool prg_man { get; set; }
        //
        public static string MandNummer { get; set; }
        //
        public static string SQLProvider { get; set; }
        public static string SQLConnStr { get; set; }

        // ####################
        // Methods
        // ####################
    }

    public static class KaJour_Global_CE {
        public static string SQLProvider { get; set; }
        public static string SQLConnStr { get; set; }
    }

    public static class KaJour_Global_LITE {
        public static string SQLProvider { get; set; }
        public static string SQLConnStr { get; set; }
    }
}
