using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;

namespace TaffyScript.Speech
{
    [WeakObject]
    public class Grammar : ITsInstance
    {
        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public string ObjectType => "Grammar";
        public GrammarBuilder GrammarBuilder { get; }

        public Grammar(TsObject[] args)
        {
            if (args is null || args.Length == 0)
                GrammarBuilder = new GrammarBuilder();
            else
                GrammarBuilder = new GrammarBuilder((string)args[0]);
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch(scriptName)
            {
                case "append_choices":
                    return append_choices(null, args);
                case "append_dictation":
                    return append_dictation(null, args);
                case "append_dictation_category":
                    return append_dictation_category(null, args);
                case "append_grammar":
                    return append_grammar(null, args);
                case "append_grammar_repeat":
                    return append_grammar_repeat(null, args);
                case "append_phrase":
                    return append_phrase(null, args);
                case "append_phrase_repeat":
                    return append_phrase_repeat(null, args);
                case "append_rule_reference":
                    return append_rule_reference(null, args);
                case "append_rule_reference_rule":
                    return append_rule_reference_rule(null, args);
                case "append_subset_matching_mode":
                    return append_subset_matching_mode(null, args);
                case "append_wildcard":
                    return append_wildcard(null, args);
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
                    del = new TsDelegate(append_choices, "append_choices", this);
                    return true;
                case "append_dictation":
                    del = new TsDelegate(append_dictation, "append_dictation", this);
                    return true;
                case "append_dictation_category":
                    del = new TsDelegate(append_dictation_category, "append_dictation_category", this);
                    return true;
                case "append_grammar":
                    del = new TsDelegate(append_grammar, "append_grammar", this);
                    return true;
                case "append_grammar_repeat":
                    del = new TsDelegate(append_grammar_repeat, "append_grammar_repeat", this);
                    return true;
                case "append_phrase":
                    del = new TsDelegate(append_phrase, "append_phrase", this);
                    return true;
                case "append_phrase_repeat":
                    del = new TsDelegate(append_phrase_repeat, "append_phrase_repeat", this);
                    return true;
                case "append_rule_reference":
                    del = new TsDelegate(append_rule_reference, "append_rule_reference", this);
                    return true;
                case "append_rule_reference_rule":
                    del = new TsDelegate(append_rule_reference_rule, "append_rule_reference_rule", this);
                    return true;
                case "append_subset_matching_mode":
                    del = new TsDelegate(append_subset_matching_mode, "append_subset_matching_mode", this);
                    return true;
                case "append_wildcard":
                    del = new TsDelegate(append_wildcard, "append_wildcard", this);
                    return true;
                default:
                    del = null;
                    return false;
            }
        }

        public TsObject append_choices(ITsInstance inst, TsObject[] args)
        {
            if (args.Length == 0)
                throw new InvalidOperationException($"There must be at least one choice to append to the {ObjectType}");
            var choices = new string[args.Length];
            for (var i = 0; i < args.Length; i++)
                choices[i] = (string)args[i];
            GrammarBuilder.Append(new Choices(choices));
            return this;
        }

        public TsObject append_grammar(ITsInstance inst, TsObject[] args)
        {
            GrammarBuilder.Append(((Grammar)args[0]).GrammarBuilder);
            return this;
        }

        public TsObject append_grammar_repeat(ITsInstance inst, TsObject[] args)
        {
            GrammarBuilder.Append(((Grammar)args[0]).GrammarBuilder, (int)args[1], (int)args[2]);
            return this;
        }

        public TsObject append_phrase(ITsInstance inst, TsObject[] args)
        {
            GrammarBuilder.Append((string)args[0]);
            return this;
        }

        public TsObject append_phrase_repeat(ITsInstance inst, TsObject[] args)
        {
            GrammarBuilder.Append((string)args[0], (int)args[1], (int)args[2]);
            return this;
        }

        public TsObject append_subset_matching_mode(ITsInstance inst, TsObject[] args)
        {
            GrammarBuilder.Append((string)args[0], (SubsetMatchingMode)(float)args[1]);
            return this;
        }

        public TsObject append_dictation(ITsInstance inst, TsObject[] args)
        {
            GrammarBuilder.AppendDictation();
            return this;
        }

        public TsObject append_dictation_category(ITsInstance inst, TsObject[] args)
        {
            GrammarBuilder.AppendDictation((string)args[0]);
            return this;
        }

        public TsObject append_rule_reference(ITsInstance inst, TsObject[] args)
        {
            GrammarBuilder.AppendRuleReference((string)args[0]);
            return this;
        }

        public TsObject append_rule_reference_rule(ITsInstance inst, TsObject[] args)
        {
            GrammarBuilder.AppendRuleReference((string)args[0], (string)args[1]);
            return this;
        }

        public TsObject append_wildcard(ITsInstance inst, TsObject[] args)
        {
            GrammarBuilder.AppendWildcard();
            return this;
        }

        public static implicit operator TsObject(Grammar grammar)
        {
            return new TsObject(grammar);
        }

        public static explicit operator Grammar(TsObject obj)
        {
            return (Grammar)obj.Value.WeakValue;
        }
    }
}
