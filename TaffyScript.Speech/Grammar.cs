using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using InternalGrammar = System.Speech.Recognition.Grammar;

namespace TaffyScript.Speech
{
    [TaffyScriptObject]
    public class Grammar : ITsInstance
    {
        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public string ObjectType => "TaffyScript.Speech.Grammar";
        public InternalGrammar Source { get; }

        public Grammar(TsObject[] args)
        {
            Source = new InternalGrammar(((GrammarBuilder)args[0]).Source);
            if (args.Length == 2)
                Source.Name = (string)args[0];
        }

        public Grammar(InternalGrammar grammar)
        {
            Source = grammar;
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            throw new MissingMethodException(ObjectType, scriptName);
        }

        public TsDelegate GetDelegate(string delegateName)
        {
            if (TryGetDelegate(delegateName, out var del))
                return del;
            throw new MissingMethodException(ObjectType, delegateName);
        }

        public TsObject GetMember(string name)
        {
            switch(name)
            {
                case "enabled":
                    return Source.Enabled;
                case "loaded":
                    return Source.Loaded;
                case "name":
                    return Source.Name;
                case "priority":
                    return Source.Priority;
                case "rule_name":
                    return Source.RuleName;
                case "weight":
                    return Source.Weight;
                default:
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public void SetMember(string name, TsObject value)
        {
            switch (name)
            {
                case "enabled":
                    Source.Enabled = (bool)value;
                    break;
                case "name":
                    Source.Name = (string)value;
                    break;
                case "priority":
                    Source.Priority = (int)value;
                    break;
                case "weight":
                    Source.Weight = (float)value;
                    break;
                default:
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public bool TryGetDelegate(string delegateName, out TsDelegate del)
        {
            throw new MissingMethodException(ObjectType, delegateName);
        }

        public static implicit operator TsObject(Grammar grammar)
        {
            return new TsInstanceWrapper(grammar);
        }

        public static explicit operator Grammar(TsObject obj)
        {
            return (Grammar)obj.WeakValue;
        }
    }
}
