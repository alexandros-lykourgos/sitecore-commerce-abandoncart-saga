// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SampleArgument.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2017
// </copyright>
// <summary>
//   SampleArgument pipeline argument.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Foundation.Commerce.AbandonCarts.Engine
{
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Conditions;

    public class SampleArgument : PipelineArgument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SampleArgument"/> class.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        public SampleArgument(object parameter)
        {
            Condition.Requires(parameter).IsNotNull("The parameter can not be null");

            this.Parameter = parameter;
        }

        /// <summary>
        /// Gets or sets the parameter.
        /// </summary>
        public object Parameter { get; set; }
    }
}
