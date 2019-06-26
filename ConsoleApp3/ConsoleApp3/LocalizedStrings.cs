using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace ThreeForms
{
    [DataContract]
    class LocalizedStrings
    {
        [DataMember]
        public string title = "Irregular verb forms",
            continueGame = "Continue",
            newSession = "New session",
            settings = "Settings",
            exit = "Exit",
            helpString = "ARROWS - navigation, ENTER - selection, ESC - exit",
            exception = "Exception",
            score = "Score",
            left = "Left",
            correct = "Correct",
            incorrect = "Incorrect";
    }
}
