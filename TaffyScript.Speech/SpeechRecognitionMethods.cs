using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;

namespace TaffyScript.Speech
{
    [TaffyScriptBaseType]
    public static class SpeechRecognitionMethods
    {
        [TaffyScriptMethod]
        public static TsObject speech_get_installed_recognizers(TsObject[] args)
        {
            var infos = SpeechRecognitionEngine.InstalledRecognizers();
            var result = new TsObject[infos.Count];
            for (var i = 0; i < result.Length; i++)
                result[i] = SpeechRecognizer.CreateRecognizerInfo(infos[i]);
            return result;
        }
    }
}
