using CombatLogExporter.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace CombatLogExporter.Reporting
{
    class TooltipReporting : CombatReporting
    {
        private LinkedList<string> CallChildren(Console.ConsoleMessage message, CombatConfiguration configuration, LinkedList<string> accumulator)
        {
            if (!message.IsVerboseEmpty)
            {
                string basic = ReplaceTags(message.m_message, configuration);
                string advanced = ReplaceTags(message.m_verbosemessage, configuration);

                accumulator.AddLast($"Combat message:\n{basic}Tooltip:\n{advanced}");
            }
            else
            {
                accumulator.AddLast(ReplaceTags(message.m_message, configuration));
            }

            Main.Log(accumulator.Last.Value);

            if (message.Children?.Count > 0)
            {
                foreach (Console.ConsoleMessage childMessage in message.Children)
                {
                    accumulator = CallChildren(childMessage, configuration, accumulator);
                }
            }

            return accumulator;
        }

        /// <summary>
        /// Return both the basic and tooltip combat information
        /// </summary>
        /// <param name="message">The combat message</param>
        /// <param name="configuration">The configuration for the combat log exporter</param>
        /// <returns>The formated combat message</returns>
        public override string HandleMessage(Console.ConsoleMessage message, CombatConfiguration configuration)
        {
            LinkedList<string> stringList = new LinkedList<string>();

            LinkedList<string> listOfStrings = CallChildren(message, configuration, stringList);
            return string.Join("\n", listOfStrings.ToArray());
        }
    }
}
