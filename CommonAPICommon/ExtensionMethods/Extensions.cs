using CommonAPICommon.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonAPICommon.ExtensionMethods
{
    public static class Extensions
  {
    /// <summary>
    /// Converts a single of Database Type to a collection of our DataContract type
    /// </summary>
    /// <param name="vinMasters">VinmasterLastest</param>
    /// <param name="passedVIN">string of the originally passed VIN</param>
    /// <returns>List<VINItem></returns>
    public static List<VINItemDto> ToVinItemList(this VINMasterWithMakeDto vinMasterLatest, string passedVIN)
    {
      var viList = new List<VINItemDto>();
      viList.AddToVINItemList(vinMasterLatest, passedVIN);
      return viList.ToList();
    }

    /// <summary>
    /// Converts a collection of Database Type to a collection of our DataContract type
    /// </summary>
    /// <param name="vinMasters">IEnumerable<VinmasterLastest></param>
    /// <param name="passedVIN">string of the originally passed VIN</param>
    /// <returns>List<VINItem></returns>
    public static List<VINItemDto> ToVinItemList(this IEnumerable<VINMasterWithMakeDto> vinMasterLatests, string passedVIN)
    {
      var viList = new List<VINItemDto>();
      foreach (var vinMasterLatest in vinMasterLatests)
      {
        viList.AddToVINItemList(vinMasterLatest, passedVIN);
      }
      return viList.ToList();
    }

    /// <summary>
    /// Compares strings character by character and returns an integer count of the columns that matched. 
    /// </summary>
    /// <param name="firstVIN">string of first VIN to be compared</param>
    /// <param name="secondVIN">string of second VIN to be compared</param>
    /// <param name="startIndex">O based integer index of character position to begin compare</param>
    /// <param name="maxLength">Max legnth of strings to be compared.  If both strings are 20 chars long and 
    /// this is set to 10.  Will only compare through 10.</param>
    /// <returns>int</returns>
    public static int MatchedDigits(this string firstVIN, string secondVIN, int startIndex, int maxLength)
    {
      int minLength = Math.Min(Math.Min(firstVIN.Length, secondVIN.Length), maxLength);  //Want the min of all strings up to the toDigit (usually 9 or 10).  
      if (minLength < startIndex - 1)
        return 0;

      int matchedCount = 0;
      for (int i = startIndex; i < minLength; i++)
      {
        if (firstVIN[i] == secondVIN[i])
          matchedCount++;
      }
      return matchedCount;
    }

    /// <summary>
    /// Counts the number of Ampersands '&' in a given string
    /// </summary>
    /// <param name="vin">string to be checked</param>
    /// <returns>int</returns>
    public static int AmpCount(this string vin)
    {
      return vin.ToCharArray().Count(c => c == '&');
    }

    // This was redundant among both overloads of ToVinItemList(), so I broke it out.
    private static void AddToVINItemList(this List<VINItemDto> viList, VINMasterWithMakeDto vinMasterLatest, string passedVIN)
    {
      viList.Add(new VINItemDto()
      {
        PassedVIN = passedVIN,
        MatchedVIN = vinMasterLatest.VIN,
        ModelYear = vinMasterLatest.ModelYear,
        EffDate = vinMasterLatest.EffDate,
        MakeCode = vinMasterLatest.MakeCode,
        Make = vinMasterLatest.Make,
        MakeId = vinMasterLatest.MakeID,
        ShortModel = vinMasterLatest.ShortModel,
        FullModel = vinMasterLatest.FullModel,
        ISOSymbol = vinMasterLatest.ISOSymbol,
        CompSymbol = vinMasterLatest.CompSymbol,
        CollSymbol = vinMasterLatest.CollSymbol,
        BiSymbol = vinMasterLatest.BiSymbol,
        PDSymbol = vinMasterLatest.PDSymbol,
        MedPaySymbol = vinMasterLatest.MedPaySymbol,
        PIPSymbol = vinMasterLatest.PIPSymbol,
        VinId = vinMasterLatest.VinID,
        RestraintInd = vinMasterLatest.RestraintInd,
        AntiTheftInd = vinMasterLatest.AntiTheftInd,
        FourWheelDriveInd = vinMasterLatest.FourWheelDriveInd,
        ISONumber = vinMasterLatest.ISONumber,
        Cylinders = vinMasterLatest.Cylinders,
        EngineType = vinMasterLatest.EngineType,
        EngineSize = vinMasterLatest.EngineSize,
        EngineInfo = vinMasterLatest.EngineInfo,
        ClassCode = vinMasterLatest.ClassCode,
        DaytimeRunningLightsInd = vinMasterLatest.DaytimeRunningLightsInd,
        AntiLockInd = vinMasterLatest.AntiLockInd,
        WheelbaseInfo = vinMasterLatest.WheelbaseInfo,
        BodyStyle = vinMasterLatest.BodyStyle,
        BodyStyleDesc = vinMasterLatest.BodyStyleDesc,
        TransmissionInfo = vinMasterLatest.TransmissionInfo,
        StateException = vinMasterLatest.StateException,
        NCIC_Manufacturer = vinMasterLatest.NCIC_Manufacturer,
        SpecialInfoSelector = vinMasterLatest.SpecialInfoSelector,
        LiabilitySymbol = vinMasterLatest.LiabilitySymbol
      });
    }
  }
}
