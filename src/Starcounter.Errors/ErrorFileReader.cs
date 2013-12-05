// ***********************************************************************
// <copyright file="ErrorFileReader.cs" company="Starcounter AB">
//     Copyright (c) Starcounter AB.  All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace Starcounter.Errors
{
    /// <summary>
    /// Class ErrorFileReader
    /// </summary>
    public static class ErrorFileReader
    {
        /// <summary>
        /// The multiple whitespace
        /// </summary>
        private static readonly Regex MultipleWhitespace = new Regex(@"\s+");

        /// <summary>
        /// Reads the error codes.
        /// </summary>
        /// <param name="instream">The instream.</param>
        /// <returns>ErrorFile.</returns>
        public static ErrorFile ReadErrorCodes(Stream instream)
        {
            XmlReaderSettings settings;
            XmlDocument document;
            List<ErrorCode> allCodes;

            settings = new XmlReaderSettings();
            settings.CloseInput = true;
            settings.IgnoreComments = true;
            settings.IgnoreWhitespace = false;
            settings.ProhibitDtd = false;
            
            document = new XmlDocument();
            document.Load(XmlReader.Create(instream, settings));
            document.Normalize();
            
            allCodes = new List<ErrorCode>();
            foreach (XmlNode fnode in document.GetElementsByTagName("facility"))
            {
                Facility facility = NodeToFacility(fnode);
                foreach (XmlNode cnode in fnode.ChildNodes)
                {
                    if (!(cnode is XmlElement))
                    {
                        continue;
                    }
                    allCodes.Add(NodeToErrorCode(cnode, facility));
                }
            }

            return new ErrorFile(allCodes);
        }

        /// <summary>
        /// Nodes to error code.
        /// </summary>
        /// <param name="cnode">The cnode.</param>
        /// <param name="facility">The facility.</param>
        /// <returns>ErrorCode.</returns>
        /// <exception cref="System.FormatException"></exception>
        static ErrorCode NodeToErrorCode(XmlNode cnode, Facility facility)
        {
            XmlElement e;
            string name;
            ushort code;
            List<string> remparams;
            XmlNode msgnode;
            XmlNode remnode;

            e = (XmlElement)cnode;
            name = e.GetAttribute("name");
            
            if (!ushort.TryParse(e.GetAttribute("hex"), NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture, out code))
            {
                throw new FormatException();
            }

            remparams = new List<string>();
            msgnode = cnode.SelectSingleNode("./message");
            remnode = cnode.SelectSingleNode("./remarks");
            if (remnode != null)
            {
                foreach (XmlNode pnode in remnode.ChildNodes)
                {
                    remparams.Add(TrimSpacesAndLineBreaks(pnode.InnerText));
                }
            }

            return new ErrorCode(
                facility,
                name,
                code,
                (Severity)Enum.Parse(typeof(Severity),
                e.GetAttribute("severity")),
                TrimSpacesAndLineBreaks(msgnode.InnerText),
                remparams
            );
        }

        /// <summary>
        /// Nodes to facility.
        /// </summary>
        /// <param name="fnode">The fnode.</param>
        /// <returns>Facility.</returns>
        /// <exception cref="System.FormatException"></exception>
        static Facility NodeToFacility(XmlNode fnode)
        {
            XmlElement e;
            string name;
            uint code;

            e = (XmlElement)fnode;
            name = e.GetAttribute("name");
            
            if (!uint.TryParse(e.GetAttribute("hex"), NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture, out code))
            {
                throw new FormatException();
            }

            return new Facility(name, code);
        }

        /// <summary>
        /// Trims the spaces and line breaks.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>System.String.</returns>
        static string TrimSpacesAndLineBreaks(string s)
        {
            return MultipleWhitespace.Replace(s, " ").Trim(' ', '\r', '\n');
        }
    }
}
