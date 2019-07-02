using System;

namespace RulesEngine.Rules
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class CompareLayerTypeAttribute : ValidationAttribute
    {
        public LayersType layerTypeBelow { get; set; }
        public CompareLayerTypeAttribute() : base()
        {

        }
        public CompareLayerTypeAttribute(string name, string message , LayersType Below)
            : base(name, message)
        {
            layerTypeBelow = Below;
        }

        public override BrokenRule Validate(object value, ValidationContext context)
        {
            BrokenRule rule = new BrokenRule();

            var targetField = context.SourceObject.GetType().GetProperty(this.Name);
            UnityEngine.Debug.Log(value +" = "+ layerTypeBelow);

            if ((LayersType)value == layerTypeBelow)
            {
                rule.IsBroken = true;
                rule.ErrorMessage = this.Message;
                rule.Name = this.Name;
            }

            return rule;
        }
    }
}
