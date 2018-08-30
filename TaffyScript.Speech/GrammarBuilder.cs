using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using InternalBuilder = System.Speech.Recognition.GrammarBuilder;

namespace TaffyScript.Speech
{
    [TaffyScriptObject]
    public class GrammarBuilder : ITsInstance
    {
        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public string ObjectType => "Grammar";
        public InternalBuilder Source { get; }

        public GrammarBuilder(TsObject[] args)
        {
            Source = new InternalBuilder();
            if (args != null && args.Length > 0)
                Source.Culture = new System.Globalization.CultureInfo((string)args[0]);
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch(scriptName)
            {
                case "append_choices":
                    return append_choices(args);
                case "append_dictation":
                    return append_dictation(args);
                case "append_dictation_category":
                    return append_dictation_category(args);
                case "append_grammar":
                    return append_grammar(args);
                case "append_grammar_repeat":
                    return append_grammar_repeat(args);
                case "append_phrase":
                    return append_phrase(args);
                case "append_phrase_repeat":
                    return append_phrase_repeat(args);
                case "append_rule_reference":
                    return append_rule_reference(args);
                case "append_rule_reference_rule":
                    return append_rule_reference_rule(args);
                case "append_subset_matching_mode":
                    return append_subset_matching_mode(args);
                case "append_wildcard":
                    return append_wildcard(args);
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
            if (TryGetDelegate(name, out var del))
                return del;
            throw new MissingMemberException(ObjectType, name);
        }

        public void SetMember(string name, TsObject value)
        {
            throw new MissingMemberException(ObjectType, name);
        }

        public bool TryGetDelegate(string delegateName, out TsDelegate del)
        {
            switch (delegateName)
            {
                case "append_choices":
                    del = new TsDelegate(append_choices, "append_choices");
                    return true;
                case "append_dictation":
                    del = new TsDelegate(append_dictation, "append_dictation");
                    return true;
                case "append_dictation_category":
                    del = new TsDelegate(append_dictation_category, "append_dictation_category");
                    return true;
                case "append_grammar":
                    del = new TsDelegate(append_grammar, "append_grammar");
                    return true;
                case "append_grammar_repeat":
                    del = new TsDelegate(append_grammar_repeat, "append_grammar_repeat");
                    return true;
                case "append_phrase":
                    del = new TsDelegate(append_phrase, "append_phrase");
                    return true;
                case "append_phrase_repeat":
                    del = new TsDelegate(append_phrase_repeat, "append_phrase_repeat");
                    return true;
                case "append_rule_reference":
                    del = new TsDelegate(append_rule_reference, "append_rule_reference");
                    return true;
                case "append_rule_reference_rule":
                    del = new TsDelegate(append_rule_reference_rule, "append_rule_reference_rule");
                    return true;
                case "append_subset_matching_mode":
                    del = new TsDelegate(append_subset_matching_mode, "append_subset_matching_mode");
                    return true;
                case "append_wildcard":
                    del = new TsDelegate(append_wildcard, "append_wildcard");
                    return true;
                default:
                    del = null;
                    return false;
            }
        }

        public TsObject append_choices(TsObject[] args)
        {
            if (args.Length == 0)
                throw new InvalidOperationException($"There must be at least one choice to append to the {ObjectType}");
            var choices = new string[args.Length];
            for (var i = 0; i < args.Length; i++)
                choices[i] = (string)args[i];
            Source.Append(new Choices(choices));
            return this;
        }

        public TsObject append_grammar(TsObject[] args)
        {
            Source.Append(((GrammarBuilder)args[0]).Source);
            return this;
        }

        public TsObject append_grammar_repeat(TsObject[] args)
        {
            Source.Append(((GrammarBuilder)args[0]).Source, (int)args[1], (int)args[2]);
            return this;
        }

        public TsObject append_phrase(TsObject[] args)
        {
            Source.Append((string)args[0]);
            return this;
        }

        public TsObject append_phrase_repeat(TsObject[] args)
        {
            Source.Append((string)args[0], (int)args[1], (int)args[2]);
            return this;
        }

        public TsObject append_subset_matching_mode(TsObject[] args)
        {
            Source.Append((string)args[0], (SubsetMatchingMode)(float)args[1]);
            return this;
        }

        public TsObject append_dictation(TsObject[] args)
        {
            Source.AppendDictation();
            return this;
        }

        public TsObject append_dictation_category(TsObject[] args)
        {
            Source.AppendDictation((string)args[0]);
            return this;
        }

        public TsObject append_rule_reference(TsObject[] args)
        {
            Source.AppendRuleReference((string)args[0]);
            return this;
        }

        public TsObject append_rule_reference_rule(TsObject[] args)
        {
            Source.AppendRuleReference((string)args[0], (string)args[1]);
            return this;
        }

        public TsObject append_wildcard(TsObject[] args)
        {
            Source.AppendWildcard();
            return this;
        }

        public static implicit operator TsObject(GrammarBuilder grammar)
        {
            return new TsInstanceWrapper(grammar);
        }

        public static explicit operator GrammarBuilder(TsObject obj)
        {
            return (GrammarBuilder)obj.WeakValue;
        }
    }
}
