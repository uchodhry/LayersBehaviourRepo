﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RulesEngine.Rules
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class RequiredFieldAttribute : ValidationAttribute
    {
        public RequiredFieldAttribute() : base()
        {

        }
        public RequiredFieldAttribute(string name, string message) : base(name, message)
        {

        }

        public override BrokenRule Validate(object value, ValidationContext context)
        {
            BrokenRule rule = new BrokenRule();
            Debug.Log("test");
            if (null == value || string.IsNullOrWhiteSpace(value.ToString()) || (string)value == "usman")
            {
                rule.IsBroken = true;
                rule.ErrorMessage = this.Message;
                rule.Name = this.Name;
            }

            return rule;
        }
    }
}
