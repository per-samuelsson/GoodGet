// ***********************************************************************
// <copyright file="Facility.cs" company="Starcounter AB">
//     Copyright (c) Starcounter AB.  All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Starcounter.Errors
{
    /// <summary>
    /// Class Facility
    /// </summary>
    public sealed class Facility
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Facility" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="code">The code.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">code;Not a valid 12-bit value: 0x</exception>
        internal Facility(string name, uint code)
        {
            if (code >> 12 != 0)
                throw new ArgumentOutOfRangeException("code", code, "Not a valid 12-bit value: 0x" + code.ToString("X"));

            this.Name = name;
            this.Code = code;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public uint Code
        {
            get;
            set;
        }
    }
}