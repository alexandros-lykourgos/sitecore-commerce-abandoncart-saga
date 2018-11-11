// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SampleModel.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2017
// </copyright>
// <summary>
//   SampleModel model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Foundation.Commerce.AbandonCarts.Engine
{
    using Sitecore.Commerce.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class SampleModel : Model
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SampleModel"/> class.
        /// </summary>
        public SampleModel()
        {
            Id = "";
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; private set; }
    }
}
