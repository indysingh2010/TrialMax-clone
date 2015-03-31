using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTI.Trialmax.Encode
{
    public class CFFMpegSource
    {
        public string m_strSourceFile;
        public string m_strDestinationFile;
        public double m_dStartTime;
        public double m_dEndTime;

        public CFFMpegSource(string sourceFile, string destinationFile, double startTime, double endTime)
        {
            m_strSourceFile = sourceFile;
            m_strDestinationFile = destinationFile;
            m_dStartTime = startTime;
            m_dEndTime = endTime;
        }
    }
}
