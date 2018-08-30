using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.Synthesis;
using TaffyScript.Collections;

namespace TaffyScript.Speech
{
    [TaffyScriptObject]
    public class SpeechSynthesizer : ITsInstance
    {
        private const string PLS = "application/pls+xml";

        private System.Speech.Synthesis.SpeechSynthesizer _synth;
        private EventCache<TtsEventType> _eventCache = new EventCache<TtsEventType>();

        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public string ObjectType => "obj_speech_synthesizer";

        public SpeechSynthesizer(TsObject[] args)
        {
            _synth = new System.Speech.Synthesis.SpeechSynthesizer();
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch(scriptName)
            {
                case "add_lexicon":
                    return add_lexicon(args);
                case "cancel":
                    return cancel(args);
                case "dispose":
                    return dispose(args);
                case "get_all_voices":
                    return get_all_voices(args);
                case "get_voices":
                    return get_voices(args);
                case "pause":
                    return pause(args);
                case "remove_lexicon":
                    return remove_lexicon(args);
                case "resume":
                    return resume(args);
                case "set_voice":
                    return set_voice(args);
                case "speak":
                    return speak(args);
                case "speak_async":
                    return speak_async(args);
                case "subscribe_event":
                    return subscribe_event(args);
                case "unsubscribe_event":
                    return unsubscribe_event(args);
                default:
                    throw new MissingMemberException(ObjectType, scriptName);
            }
        }

        public TsDelegate GetDelegate(string delegateName)
        {
            if (TryGetDelegate(delegateName, out var del))
                return del;
            throw new MissingMemberException(ObjectType, delegateName);
        }

        public TsObject GetMember(string name)
        {
            switch(name)
            {
                case "rate":
                    return _synth.Rate;
                case "state":
                    return (float)_synth.State;
                case "voice":
                    return CreateVoice(_synth.Voice);
                case "volume":
                    return _synth.Volume;
                default:
                    if (TryGetDelegate(name, out var del))
                        return del;
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public void SetMember(string name, TsObject value)
        {
            switch (name)
            {
                case "rate":
                    _synth.Rate = (int)value;
                    break;
                case "voice":
                    set_voice(new[] { value });
                    break;
                case "volume":
                    _synth.Volume = (int)value;
                    break;
                default:
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public bool TryGetDelegate(string delegateName, out TsDelegate del)
        {
            switch (delegateName)
            {
                case "add_lexicon":
                    del = new TsDelegate(add_lexicon, "add_lexicon");
                    return true;
                case "cancel":
                    del = new TsDelegate(cancel, "cancel");
                    return true;
                case "dispose":
                    del = new TsDelegate(dispose, "dispose");
                    return true;
                case "get_all_voices":
                    del = new TsDelegate(get_all_voices, "get_all_voices");
                    return true;
                case "get_voices":
                    del = new TsDelegate(get_voices, "get_voices");
                    return true;
                case "pause":
                    del = new TsDelegate(pause, "pause");
                    return true;
                case "remove_lexicon":
                    del = new TsDelegate(remove_lexicon, "remove_lexicon");
                    return true;
                case "resume":
                    del = new TsDelegate(resume, "resume");
                    return true;
                case "set_voice":
                    del = new TsDelegate(set_voice, "set_voice");
                    return true;
                case "speak":
                    del = new TsDelegate(speak, "speak");
                    return true;
                case "speak_async":
                    del = new TsDelegate(speak_async, "speak_async");
                    return true;
                case "subscribe_event":
                    del = new TsDelegate(subscribe_event, "subscribe_event");
                    return true;
                case "unsubscribe_event":
                    del = new TsDelegate(unsubscribe_event, "unsubscribe_event");
                    return true;
                default:
                    del = null;
                    return false;
            }
        }

        public TsObject get_all_voices(TsObject[] args)
        {
            var voices = new List<TsObject>();
            foreach(var voiceInfo in _synth.GetInstalledVoices().Where(v => v.Enabled).Select(v => v.VoiceInfo))
                voices.Add(CreateVoice(voiceInfo));
            return voices.ToArray();
        }

        public TsObject get_voices(TsObject[] args)
        {
            CultureInfo culture;
            if (args.Length > 0)
                culture = new CultureInfo((string)args[0]);
            else
                culture = CultureInfo.CurrentCulture;
            var voices = new List<TsObject>();

            foreach (var voiceInfo in _synth.GetInstalledVoices(culture).Where(v => v.Enabled).Select(v => v.VoiceInfo))
                voices.Add(CreateVoice(voiceInfo));
            return voices.ToArray();
        }

        public TsObject set_voice(TsObject[] args)
        {
            _synth.SelectVoice((string)args[0]);
            return TsObject.Empty;
        }

        public TsObject add_lexicon(TsObject[] args)
        {
            _synth.AddLexicon(new Uri((string)args[0]), PLS);
            return TsObject.Empty;
        }

        public TsObject remove_lexicon(TsObject[] args)
        {
            _synth.RemoveLexicon(new Uri((string)args[0]));
            return TsObject.Empty;
        }

        public TsObject speak(TsObject[] args)
        {
            var text = (string)args[0];
            if (string.IsNullOrWhiteSpace(text))
                return false;

            _synth.Speak(text);
            return true;
        }

        public TsObject speak_async(TsObject[] args)
        {
            var text = (string)args[0];
            if (string.IsNullOrWhiteSpace(text))
                return false;

            _synth.SpeakAsync(text);
            return true;
        }

        public TsObject pause(TsObject[] args)
        {
            _synth.Pause();
            return _synth.State == SynthesizerState.Paused;
        }

        public TsObject resume(TsObject[] args)
        {
            _synth.Resume();
            return TsObject.Empty;
        }

        public TsObject cancel(TsObject[] args)
        {
            _synth.SpeakAsyncCancelAll();
            return TsObject.Empty;
        }

        public TsObject dispose(TsObject[] args)
        {
            _synth.Dispose();
            return TsObject.Empty;
        }

        public TsObject subscribe_event(TsObject[] args)
        {
            var eventType = (TtsEventType)(float)args[0];
            var callback = (TsDelegate)args[1];
            switch(eventType)
            {
                case TtsEventType.VisemeReached:
                    VisemeReached(callback);
                    break;
                case TtsEventType.PhonemeReached:
                    PhonemeReached(callback);
                    break;
                case TtsEventType.SpeakCompleted:
                    SpeakCompleted(callback);
                    break;
                case TtsEventType.SpeakProgress:
                    SpeakProgress(callback);
                    break;
                case TtsEventType.SpeakStarted:
                    SpeakStarted(callback);
                    break;
                case TtsEventType.StateChanged:
                    StateChanged(callback);
                    break;
                case TtsEventType.VoiceChanged:
                    VoiceChanged(callback);
                    break;
                default:
                    throw new InvalidOperationException($"{ObjectType} does not define event {eventType}");
            }
            return TsObject.Empty;
        }

        public TsObject unsubscribe_event(TsObject[] args)
        {
            var eventType = (TtsEventType)(float)args[0];
            var callback = (TsDelegate)args[1];
            if (!_eventCache.TryRemove(eventType, callback, out var handler))
                return false;

            switch(eventType)
            {
                case TtsEventType.PhonemeReached:
                    _synth.PhonemeReached -= (EventHandler<PhonemeReachedEventArgs>)handler;
                    break;
                case TtsEventType.SpeakCompleted:
                    _synth.SpeakCompleted -= (EventHandler<SpeakCompletedEventArgs>)handler;
                    break;
                case TtsEventType.SpeakProgress:
                    _synth.SpeakProgress -= (EventHandler<SpeakProgressEventArgs>)handler;
                    break;
                case TtsEventType.SpeakStarted:
                    _synth.SpeakStarted -= (EventHandler<SpeakStartedEventArgs>)handler;
                    break;
                case TtsEventType.StateChanged:
                    _synth.StateChanged -= (EventHandler<StateChangedEventArgs>)handler;
                    break;
                case TtsEventType.VisemeReached:
                    _synth.VisemeReached -= (EventHandler<VisemeReachedEventArgs>)handler;
                    break;
                case TtsEventType.VoiceChanged:
                    _synth.VoiceChange -= (EventHandler<VoiceChangeEventArgs>)handler;
                    break;
                default:
                    return false;
            }

            return true;
        }

        private void VisemeReached(TsDelegate del)
        {
            EventHandler<VisemeReachedEventArgs> handler = (s, e) =>
            {
                var result = new DynamicInstance("VisemeReached");
                result["viseme"] = e.Viseme;
                del.Invoke(new TsObject[] { result });
            };
            _synth.VisemeReached += handler;
            _eventCache.Cache(TtsEventType.VisemeReached, del, handler);
        }

        private void PhonemeReached(TsDelegate del)
        {
            EventHandler<PhonemeReachedEventArgs> handler = (s, e) =>
            {
                var result = new DynamicInstance("PhonemeReached");
                result["phoneme"] = e.Phoneme;
                del.Invoke(new TsObject[] { result });
            };
            _synth.PhonemeReached += handler;
            _eventCache.Cache(TtsEventType.PhonemeReached, del, handler);
        }

        private void SpeakCompleted(TsDelegate del)
        {
            EventHandler<SpeakCompletedEventArgs> handler = (s, e) =>
            {
                var result = new DynamicInstance("SpeakCompleted");
                result["error"] = e.Error == null ? "" : e.Error.Message;
                result["cancelled"] = e.Cancelled;
                del.Invoke(new TsObject[] { result });
            };
            _synth.SpeakCompleted += handler;
            _eventCache.Cache(TtsEventType.SpeakCompleted, del, handler);
        }

        private void SpeakProgress(TsDelegate del)
        {
            EventHandler<SpeakProgressEventArgs> handler = (s, e) =>
            {
                var result = new DynamicInstance("SpeakProgress");
                result["text"] = e.Text;
                del.Invoke(new TsObject[] { result });
            };
            _synth.SpeakProgress += handler;
            _eventCache.Cache(TtsEventType.SpeakProgress, del, handler);
        }

        private void SpeakStarted(TsDelegate del)
        {
            EventHandler<SpeakStartedEventArgs> handler = (s, e) =>
            {
                del.Invoke();
            };
            _synth.SpeakStarted += handler;
            _eventCache.Cache(TtsEventType.SpeakStarted, del, handler);
        }

        private void StateChanged(TsDelegate del)
        {
            EventHandler<StateChangedEventArgs> handler = (s, e) =>
            {
                var result = new DynamicInstance("StateChanged");
                result["previous_state"] = (float)e.PreviousState;
                result["state"] = (float)e.State;
                del.Invoke(new TsObject[] { result });
            };
            _synth.StateChanged += handler;
            _eventCache.Cache(TtsEventType.StateChanged, del, handler);
        }

        private void VoiceChanged(TsDelegate del)
        {
            EventHandler<VoiceChangeEventArgs> handler = (s, e) =>
            {
                del.Invoke(CreateVoice(e.Voice));
            };
            _synth.VoiceChange += handler;
            _eventCache.Cache(TtsEventType.VoiceChanged, del, handler);
        }

        private TsObject CreateVoice(VoiceInfo voiceInfo)
        {
            if (voiceInfo is null)
                return TsObject.Empty;

            var voice = new DynamicInstance("VoiceInfo");
            voice["name"] = voiceInfo.Name;
            voice["gender"] = (float)voiceInfo.Gender;
            voice["age"] = (float)voiceInfo.Age;
            voice["culture"] = voiceInfo.Culture.ToString();
            voice["description"] = voiceInfo.Description;
            voice["id"] = voiceInfo.Id;
            return voice;
        }

        public static implicit operator TsObject(SpeechSynthesizer synthesizer)
        {
            return new TsInstanceWrapper(synthesizer);
        }

        public static explicit operator SpeechSynthesizer(TsObject obj)
        {
            return (SpeechSynthesizer)obj.WeakValue;
        }
    }

    public enum TtsEventType
    {
        VisemeReached,
        PhonemeReached,
        SpeakCompleted,
        SpeakProgress,
        SpeakStarted,
        StateChanged,
        VoiceChanged
    }
}
