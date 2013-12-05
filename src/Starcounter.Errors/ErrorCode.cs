// ***********************************************************************
// <copyright file="ErrorCode.cs" company="Starcounter AB">
//     Copyright (c) Starcounter AB.  All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace Starcounter.Errors
{
    /// <summary>
    /// Class ErrorCode
    /// </summary>
    public sealed class ErrorCode
    {
        /// <summary>
        /// Gets or sets the facility.
        /// </summary>
        /// <value>The facility.</value>
        public Facility Facility
        {
            get;
            set;
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
        public ushort Code
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the severity.
        /// </summary>
        /// <value>The severity.</value>
        public Severity Severity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the remark paragraphs.
        /// </summary>
        /// <value>The remark paragraphs.</value>
        public IList<string> RemarkParagraphs
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name of the constant.
        /// </summary>
        /// <value>The name of the constant.</value>
        public string ConstantName
        {
            get { return Name.ToUpper(); }
        }

        /// <summary>
        /// Gets the code with facility.
        /// </summary>
        /// <value>The code with facility.</value>
        public uint CodeWithFacility
        {
            get
            {
                return (Facility.Code * 1000) + Code;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorCode" /> class.
        /// </summary>
        /// <param name="facility">The facility.</param>
        /// <param name="name">The name.</param>
        /// <param name="code">The code.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="description">The description.</param>
        /// <param name="remarkParagraphs">The remark paragraphs.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">code;Not a valid value (allowed range is 0-999): 0x</exception>
        internal ErrorCode(
            Facility facility,
            string name,
            ushort code,
            Severity severity,
            string description,
            IEnumerable<string> remarkParagraphs)
        {
            if (code > 999)
                throw new ArgumentOutOfRangeException("code", code, "Not a valid value (allowed range is 0-999): 0x" + code.ToString("X"));

            this.Facility = facility;
            this.Name = name;
            this.Code = code;
            this.Severity = severity;
            this.Description = description;
            this.RemarkParagraphs = new List<string>(remarkParagraphs).AsReadOnly();
        }
    }
}