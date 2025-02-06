using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPICommon.Dto;

namespace CommonAPIBusinessLayer.Services.Interface
{
    public interface IISOService
    {
        CoverageVerifierResponseDto CallISOCV(CoverageVerifierDto covVer, int quoteId);

        PrefillResponseDto CallISOPrefill(ApplicantDto applicant, int quoteId);
    }
}
