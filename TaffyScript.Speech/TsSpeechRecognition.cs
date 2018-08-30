using System;
using System.Globalization;
using System.Linq;
using System.Speech.Recognition;
using TaffyScript.Collections;

namespace TaffyScript.Speech
{
    [TaffyScriptObject]
    public class SpeechRecognizer : ITsInstance
    {
        private SpeechRecognitionEngine _source;
        private EventCache<RecognizerEventType> _eventCache = new EventCache<RecognizerEventType>();

        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public string ObjectType => "SpeechRecognizer";

        public SpeechRecognizer(TsObject[] args)
        {
            if (args.Length != 0)
                _source = new SpeechRecognitionEngine((string)args[0]);
            else
                _source = new SpeechRecognitionEngine();

            _source.SetInputToDefaultAudioDevice();
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch(scriptName)
            {
                case "dispose":
                    return dispose(args);
                case "emulate_recognize":
                    return emulate_recognize(args);
                case "emulate_recognize_async":
                    return emulate_recognize_async(args);
                case "load_grammar":
                    return load_grammar(args);
                case "load_grammar_async":
                    return load_grammar_async(args);
                case "query_setting":
                    return query_setting(args);
                case "recognize":
                    return recognize(args);
                case "recognize_async":
                    return recognize_async(args);
                case "recognize_async_cancel":
                    return recognize_async_cancel(args);
                case "recognize_async_stop":
                    return recognize_async_stop(args);
                case "request_recognizer_update":
                    return request_recognizer_update(args);
                case "subscribe_event":
                    return subscribe_event(args);
                case "update_setting":
                    return update_setting(args);
                case "unload_all_grammars":
                    return unload_all_grammars(args);
                case "unload_grammar":
                    return unload_grammar(args);
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
                case "audio_level":
                    return _source.AudioLevel;
                case "audio_state":
                    return (float)_source.AudioState;
                case "babble_timeout":
                    return _source.BabbleTimeout.Milliseconds;
                case "end_silence_timeout":
                    return _source.EndSilenceTimeout.Milliseconds;
                case "end_silence_timeout_ambiguous":
                    return _source.EndSilenceTimeoutAmbiguous.Milliseconds;
                case "grammars":
                    var grammars = new TsObject[_source.Grammars.Count];
                    for (var i = 0; i < grammars.Length; i++)
                        grammars[i] = _source.Grammars[i].Name;
                    return grammars;
                case "initial_silence_timeout":
                    return _source.InitialSilenceTimeout.Milliseconds;
                case "max_alternatives":
                    return _source.MaxAlternates;
                case "recognizer_info":
                    return CreateRecognizerInfo(_source.RecognizerInfo);
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
                case "babble_timeout":
                    _source.BabbleTimeout = TimeSpan.FromMilliseconds((double)value);
                    break;
                case "end_silence_timeout":
                    _source.EndSilenceTimeout = TimeSpan.FromMilliseconds((double)value);
                    break;
                case "end_silence_timeout_ambiguous":
                    _source.EndSilenceTimeoutAmbiguous = TimeSpan.FromMilliseconds((double)value);
                    break;
                case "initial_silence_timeout":
                    _source.InitialSilenceTimeout = TimeSpan.FromMilliseconds((double)value);
                    break;
                case "max_alternatives":
                    _source.MaxAlternates = (int)value;
                    break;
                default:
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public bool TryGetDelegate(string delegateName, out TsDelegate del)
        {
            switch (delegateName)
            {
                case "dispose":
                    del = new TsDelegate(dispose, "dispose");
                    return true;
                case "emulate_recognize":
                    del = new TsDelegate(emulate_recognize, "emulate_recognize");
                    return true;
                case "emulate_recognize_async":
                    del = new TsDelegate(emulate_recognize_async, "emulate_recognize_async");
                    return true;
                case "load_grammar":
                    del = new TsDelegate(load_grammar, "load_grammar");
                    return true;
                case "load_grammar_async":
                    del = new TsDelegate(load_grammar_async, "load_grammar_async");
                    return true;
                case "query_setting":
                    del = new TsDelegate(query_setting, "query_setting");
                    return true;
                case "recognize":
                    del = new TsDelegate(recognize, "recognize");
                    return true;
                case "recognize_async":
                    del = new TsDelegate(recognize_async, "recognize_async");
                    return true;
                case "recognize_async_cancel":
                    del = new TsDelegate(recognize_async_cancel, "recognize_async_cancel");
                    return true;
                case "recognize_async_stop":
                    del = new TsDelegate(recognize_async_stop, "recognize_async_stop");
                    return true;
                case "request_recognizer_update":
                    del = new TsDelegate(request_recognizer_update, "request_recognizer_update");
                    return true;
                case "subscribe_event":
                    del = new TsDelegate(subscribe_event, "subscribe_event");
                    return true;
                case "update_setting":
                    del = new TsDelegate(update_setting, "update_setting");
                    return true;
                case "unload_all_grammars":
                    del = new TsDelegate(unload_all_grammars, "unload_all_grammars");
                    return true;
                case "unload_grammar":
                    del = new TsDelegate(unload_grammar, "unload_grammar");
                    return true;
                case "unsubscribe_event":
                    del = new TsDelegate(unsubscribe_event, "unsubscribe_event");
                    return true;
                default:
                    del = null;
                    return false;
            }
        }

        public TsObject emulate_recognize(TsObject[] args)
        {
            if(args.Length == 2)
                _source.EmulateRecognize((string)args[0], (CompareOptions)(float)args[1]);
            else
                _source.EmulateRecognize((string)args[0]);
            return TsObject.Empty;
        }

        public TsObject emulate_recognize_async(TsObject[] args)
        {
            if(args.Length == 2)
                _source.EmulateRecognizeAsync((string)args[0], (CompareOptions)(float)args[1]);
            else
                _source.EmulateRecognizeAsync((string)args[0]);
            return TsObject.Empty;
        }

        public TsObject load_grammar(TsObject[] args)
        {
            _source.LoadGrammar(((Grammar)args[0]).Source);
            return TsObject.Empty;
        }

        public TsObject load_grammar_async(TsObject[] args)
        {
            _source.LoadGrammar(((Grammar)args[0]).Source);
            return TsObject.Empty;
        }

        public TsObject unload_all_grammars(TsObject[] args)
        {
            _source.UnloadAllGrammars();
            return TsObject.Empty;
        }

        public TsObject unload_grammar(TsObject[] args)
        {
            _source.UnloadGrammar(((Grammar)args[0]).Source);
            return true;
        }

        public TsObject query_setting(TsObject[] args)
        {
            var result = _source.QueryRecognizerSetting((string)args[0]);
            if (result is null)
                return TsObject.Empty;
            if (result is string s)
                return s;
            if (result is double d)
                return d;

            return TsObject.Empty;
        }

        public TsObject update_setting(TsObject[] args)
        {
            try
            {
                var name = (string)args[0];
                var value = args[1];
                if (value.Type == VariableType.Real)
                    _source.UpdateRecognizerSetting(name, (int)value.GetNumber());
                else if (value.Type == VariableType.String)
                    _source.UpdateRecognizerSetting(name, value.GetString());
                else
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public TsObject recognize(TsObject[] args)
        {
            RecognitionResult result = args.Length == 0 ? _source.Recognize() : _source.Recognize(TimeSpan.FromMilliseconds((double)args[0]));
            return result is null ? TsObject.Empty : CreateRecognitionResult(result);
        }

        public TsObject recognize_async(TsObject[] args)
        {
            if (args is null || args.Length == 0)
                _source.RecognizeAsync();
            else
                _source.RecognizeAsync((RecognizeMode)(float)args[0]);
            return TsObject.Empty;
        }

        public TsObject recognize_async_cancel(TsObject[] args)
        {
            _source.RecognizeAsyncCancel();
            return TsObject.Empty;
        }

        public TsObject recognize_async_stop(TsObject[] args)
        {
            _source.RecognizeAsyncStop();
            return TsObject.Empty;
        }

        public TsObject request_recognizer_update(TsObject[] args)
        {
            _source.RequestRecognizerUpdate();
            return TsObject.Empty;
        }

        public TsObject dispose(TsObject[] args)
        {
            _source.Dispose();
            return TsObject.Empty;
        }

        public TsObject subscribe_event(TsObject[] args)
        {
            var eventType = (RecognizerEventType)(float)args[0];
            var callback = (TsDelegate)args[1];
            switch(eventType)
            {
                case RecognizerEventType.AudioLevelUpdated:
                    AudioLevelUpdated(callback);
                    break;
                case RecognizerEventType.AudioSignalProblemOccurred:
                    AudioSignalProblemOccurred(callback);
                    break;
                case RecognizerEventType.AudioStateChanged:
                    AudioStateChanged(callback);
                    break;
                case RecognizerEventType.EmulateRecognizeCompleted:
                    EmulateRecognizeCompleted(callback);
                    break;
                case RecognizerEventType.LoadGrammarCompleted:
                    LoadGrammarCompleted(callback);
                    break;
                case RecognizerEventType.RecognizeCompleted:
                    RecognizeCompleted(callback);
                    break;
                case RecognizerEventType.RecognizerUpdateReached:
                    RecognizerUpdateReached(callback);
                    break;
                case RecognizerEventType.SpeechDetected:
                    SpeechDetected(callback);
                    break;
                case RecognizerEventType.SpeechHypothesized:
                    SpeechHypothesized(callback);
                    break;
                case RecognizerEventType.SpeechRecognitionRejected:
                    SpeechRecognitionRejected(callback);
                    break;
                case RecognizerEventType.SpeechRecognized:
                    SpeechRecognized(callback);
                    break;
                default:
                    throw new InvalidOperationException($"{ObjectType} does not define event {eventType}");
            }
            return TsObject.Empty;
        }

        public TsObject unsubscribe_event(TsObject[] args)
        {
            var eventType = (RecognizerEventType)(float)args[0];
            var callback = (TsDelegate)args[1];
            if (!_eventCache.TryRemove(eventType, callback, out var handler))
                return false;

            switch (eventType)
            {
                case RecognizerEventType.AudioLevelUpdated:
                    _source.AudioLevelUpdated -= (EventHandler<AudioLevelUpdatedEventArgs>)handler;
                    break;
                case RecognizerEventType.AudioSignalProblemOccurred:
                    _source.AudioSignalProblemOccurred -= (EventHandler<AudioSignalProblemOccurredEventArgs>)handler;
                    break;
                case RecognizerEventType.AudioStateChanged:
                    _source.AudioStateChanged -= (EventHandler<AudioStateChangedEventArgs>)handler;
                    break;
                case RecognizerEventType.EmulateRecognizeCompleted:
                    _source.EmulateRecognizeCompleted -= (EventHandler<EmulateRecognizeCompletedEventArgs>)handler;
                    break;
                case RecognizerEventType.LoadGrammarCompleted:
                    _source.LoadGrammarCompleted -= (EventHandler<LoadGrammarCompletedEventArgs>)handler;
                    break;
                case RecognizerEventType.RecognizeCompleted:
                    _source.RecognizeCompleted -= (EventHandler<RecognizeCompletedEventArgs>)handler;
                    break;
                case RecognizerEventType.RecognizerUpdateReached:
                    _source.RecognizerUpdateReached -= (EventHandler<RecognizerUpdateReachedEventArgs>)handler;
                    break;
                case RecognizerEventType.SpeechDetected:
                    _source.SpeechDetected -= (EventHandler<SpeechDetectedEventArgs>)handler;
                    break;
                case RecognizerEventType.SpeechHypothesized:
                    _source.SpeechHypothesized -= (EventHandler<SpeechHypothesizedEventArgs>)handler;
                    break;
                case RecognizerEventType.SpeechRecognitionRejected:
                    _source.SpeechRecognitionRejected -= (EventHandler<SpeechRecognitionRejectedEventArgs>)handler;
                    break;
                case RecognizerEventType.SpeechRecognized:
                    _source.SpeechRecognized -= (EventHandler<SpeechRecognizedEventArgs>)handler;
                    break;
                default:
                    return false;
            }

            return true;
        }

        private void AudioLevelUpdated(TsDelegate del)
        {
            EventHandler<AudioLevelUpdatedEventArgs> handler = (s, e) =>
            {
                var result = new DynamicInstance("AudioLevelUpdated");
                result["audio_level"] = e.AudioLevel;
                del.Invoke(new TsObject[] { result });
            };
            _source.AudioLevelUpdated += handler;
            _eventCache.Cache(RecognizerEventType.AudioLevelUpdated, del, handler);
        }

        private void AudioSignalProblemOccurred(TsDelegate del)
        {
            EventHandler<AudioSignalProblemOccurredEventArgs> handler = (s, e) =>
            {
                var result = new DynamicInstance("AudioSignalProblemOccurred");
                result["audio_signal_problem"] = (float)e.AudioSignalProblem;
                del.Invoke(new TsObject[] { result });
            };
            _source.AudioSignalProblemOccurred += handler;
            _eventCache.Cache(RecognizerEventType.AudioSignalProblemOccurred, del, handler);
        }

        private void AudioStateChanged(TsDelegate del)
        {
            EventHandler<AudioStateChangedEventArgs> handler = (s, e) =>
            {
                var result = new DynamicInstance("AudioStateChanged");
                result["audio_state"] = (float)e.AudioState;
                del.Invoke(new TsObject[] { result });
            };
            _source.AudioStateChanged += handler;
            _eventCache.Cache(RecognizerEventType.AudioStateChanged, del, handler);
        }

        private void EmulateRecognizeCompleted(TsDelegate del)
        {
            EventHandler<EmulateRecognizeCompletedEventArgs> handler = (s, e) =>
            {
                var result = new DynamicInstance("EmulateRecognizeCompleted");
                result["error"] = e.Error?.Message ?? "";
                result["cancelled"] = e.Cancelled;
                result["result"] = CreateRecognitionResult(e.Result);
                del.Invoke(new TsObject[] { result });
            };
            _source.EmulateRecognizeCompleted += handler;
            _eventCache.Cache(RecognizerEventType.EmulateRecognizeCompleted, del, handler);
        }

        private void LoadGrammarCompleted(TsDelegate del)
        {
            EventHandler<LoadGrammarCompletedEventArgs> handler = (s, e) =>
            {
                var result = new DynamicInstance("LoadGrammarCompleted");
                result["error"] = e.Error?.Message ?? "";
                result["cancelled"] = e.Cancelled;
                result["grammar"] = e.Grammar.Name;
                del.Invoke(new TsObject[] { result });
            };
            _source.LoadGrammarCompleted += handler;
            _eventCache.Cache(RecognizerEventType.LoadGrammarCompleted, del, handler);
        }

        private void RecognizeCompleted(TsDelegate del)
        {
            EventHandler<RecognizeCompletedEventArgs> handler = (s, e) =>
            {
                var result = new DynamicInstance("RecognizeCompleted");
                result["cancelled"] = e.Cancelled;
                result["error"] = e.Error?.Message ?? "";
                result["babble_timeout"] = e.BabbleTimeout;
                result["initial_silence_timeout"] = e.InitialSilenceTimeout;
                result["input_stream_ended"] = e.InputStreamEnded;
                result["result"] = CreateRecognitionResult(e.Result);
                del.Invoke(new TsObject[] { result });
            };
            _source.RecognizeCompleted += handler;
            _eventCache.Cache(RecognizerEventType.RecognizeCompleted, del, handler);
        }

        private void RecognizerUpdateReached(TsDelegate del)
        {
            EventHandler<RecognizerUpdateReachedEventArgs> handler = (s, e) =>
            {
                var result = new DynamicInstance("RecognizerUpdateReached");
                del.Invoke();
            };
            _source.RecognizerUpdateReached += handler;
            _eventCache.Cache(RecognizerEventType.RecognizerUpdateReached, del, handler);
        }

        private void SpeechDetected(TsDelegate del)
        {
            EventHandler<SpeechDetectedEventArgs> handler = (s, e) =>
            {
                var result = new DynamicInstance("SpeechDetected");
                del.Invoke();
            };
            _source.SpeechDetected += handler;
            _eventCache.Cache(RecognizerEventType.SpeechDetected, del, handler);
        }

        private void SpeechHypothesized(TsDelegate del)
        {
            EventHandler<SpeechHypothesizedEventArgs> handler = (s, e) =>
            {
                var result = new DynamicInstance("SpeechHypothesized");
                result["result"] = CreateRecognitionResult(e.Result);
                del.Invoke(new TsObject[] { result });
            };
            _source.SpeechHypothesized += handler;
            _eventCache.Cache(RecognizerEventType.SpeechHypothesized, del, handler);
        }

        private void SpeechRecognitionRejected(TsDelegate del)
        {
            EventHandler<SpeechRecognitionRejectedEventArgs> handler = (s, e) =>
            {
                var result = new DynamicInstance("SpeechRecognitionRejected");
                result["result"] = CreateRecognitionResult(e.Result);
                del.Invoke(new TsObject[] { result });
            };
            _source.SpeechRecognitionRejected += handler;
            _eventCache.Cache(RecognizerEventType.SpeechRecognitionRejected, del, handler);
        }

        private void SpeechRecognized(TsDelegate del)
        {
            EventHandler<SpeechRecognizedEventArgs> handler = (s, e) =>
            {
                var result = new DynamicInstance("SpeechRecognized");
                result["result"] = CreateRecognitionResult(e.Result);
                del.Invoke(new TsObject[] { result });
            };
            _source.SpeechRecognized += handler;
            _eventCache.Cache(RecognizerEventType.SpeechRecognized, del, handler);
        }

        private TsObject CreateRecognitionResult(RecognitionResult result)
        {
            var inst = new DynamicInstance("RecognitionResult");
            inst["text"] = result.Text ?? string.Empty;
            inst["grammar"] = result.Grammar?.Name ?? string.Empty;
            inst["confidence"] = result.Confidence;
            var alternates = new TsObject[result.Alternates.Count];
            for (var i = 0; i < alternates.Length; i++)
                alternates[i] = CreateRecognizedPhrase(result.Alternates[i]);
            inst["alternates"] = alternates;
            return inst;
        }

        private TsObject CreateRecognizedPhrase(RecognizedPhrase phrase)
        {
            var inst = new DynamicInstance("RecognizedPhrase");
            inst["text"] = phrase.Text ?? string.Empty;
            inst["grammar"] = phrase.Grammar?.Name ?? string.Empty;
            inst["confidence"] = phrase.Confidence;
            return inst;
        }

        public static TsObject CreateRecognizerInfo(RecognizerInfo info)
        {
            var inst = new DynamicInstance("RecognizerInfo");
            inst["name"] = info.Name;
            inst["id"] = info.Id;
            inst["culture"] = info.Culture.ToString();
            inst["description"] = info.Description;
            return inst;
        }
    }

    public enum RecognizerEventType
    {
        AudioLevelUpdated,
        AudioSignalProblemOccurred,
        AudioStateChanged,
        EmulateRecognizeCompleted,
        LoadGrammarCompleted,
        RecognizeCompleted,
        RecognizerUpdateReached,
        SpeechDetected,
        SpeechHypothesized,
        SpeechRecognitionRejected,
        SpeechRecognized
    }
}
