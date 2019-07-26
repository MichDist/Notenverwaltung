using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Npgsql;

namespace Menufuehrung_Vorlage
{
    class Program
    {
        // Arrays deklarieren
        static int[] ArrayAWPSchriftlich; static int[] ArrayAWPMuendlich;
        static int[] ArrayITSSchriftlich; static int[] ArrayITSMuendlich;
        static int[] ArrayVSSchriftlich; static int[] ArrayVSMuendlich;
        static int[] ArrayBWPSchriftlich; static int[] ArrayBWPMuendlich;
        static int[] ArrayEnglischSchriftlich; static int[] ArrayEnglischMuendlich;
        static int[] ArraySportSchriftlich; static int[] ArraySportMuendlich;
        static int[] ArrayDeutschSchriftlich; static int[] ArrayDeutschMuendlich;
        static int[] ArrayReligionSchriftlich; static int[] ArrayReligionMuendlich;
        static int[] ArraySozialkundeSchriftlich; static int[] ArraySozialkundeMuendlich;
        //Stundenplan
        static string[,] ArrayStundenplan;

        static void Main(string[] args)
        {

            // Variablen
            int iAuswahl = 0;
            int iFach = 0;
            char cNoteArt;
            //Stundenplan erstellen
            if (ArrayStundenplan == null)
            {
                StundenplanErstellen();
            }
            do
            {
                iAuswahl = Hauptmenu();

                // Hier wird die entpsrechende Methode abhängig von der Auswahl des Anwenders aufgerufen
                switch (iAuswahl)
                {
                    case 1:
                        // Noten eingeben und das Array dazu erstellen
                        // Fach als Zahl
                        iFach = Faecher();
                        // muendlich oder Schriftlich
                        cNoteArt = SchriftlichOderMuendlich();
                        // Arrays erstellen
                        ArrayErstellenAuswahl(iFach, cNoteArt);
                        break;

                    case 2:
                        // Noten ausgeben und Möglichkeit zum Ändern
                        // Fachauswahl
                        iFach = Faecher();
                        // Schriftlich oder muendlich
                        cNoteArt = SchriftlichOderMuendlich();
                        // Noten ausgeben
                        ArrayAusgebenUndAendern(iFach, cNoteArt);
                        break;

                    case 3:
                        // Notendurchschnitt berechnen
                        iFach = Faecher();
                        // Auswahl für die Berechnung des Durchschnitts
                        AuswahlDurchschnittBerechnen(iFach);
                        break;

                    case 4:
                        // Stundeplan ausgeben
                        StundenplanAusgeben();
                        break;

                    case 5:
                        // Stundenplan bearbeiten
                        StundenplanBearbeiten();
                        break;

                    case 6:
                        // Stundenplan in Textdatei schreiben
                        StundenplanTextdatei();
                        break;

                    case 7:
                        // Noten in Datenbank speichern
                        TestUndAuswahlDatenbank();
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Das Programm wird beendet!");
                        iAuswahl = 0;
                        break;
                }
            } while (iAuswahl != 0);

            Console.ReadLine();
        }

        static void TestUndAuswahlDatenbank()
        {
            if(ArrayAWPSchriftlich != null)
            {
                NotenSpeichernDatenbankSP(ArrayAWPSchriftlich,'m',"AWP");
                //NotenSpeichernDatenbank(ArrayAWPSchriftlich, 's', "AWP");
            }
            if(ArrayAWPMuendlich != null)
            {
                NotenSpeichernDatenbank(ArrayAWPMuendlich, 'm', "AWP");
            }
            if (ArrayITSSchriftlich != null)
            {
                NotenSpeichernDatenbank(ArrayITSSchriftlich, 's', "ITS");
            }
            if (ArrayITSMuendlich != null)
            {
                NotenSpeichernDatenbank(ArrayITSMuendlich, 'm', "ITS");
            }
            if (ArrayVSSchriftlich != null)
            {
                NotenSpeichernDatenbank(ArrayVSSchriftlich, 's', "VS");
            }
            if (ArrayVSMuendlich != null)
            {
                NotenSpeichernDatenbank(ArrayVSMuendlich, 'm', "VS");
            }
            if (ArrayBWPSchriftlich != null)
            {
                NotenSpeichernDatenbank(ArrayBWPSchriftlich, 's', "BWP");
            }
            if (ArrayBWPMuendlich != null)
            {
                NotenSpeichernDatenbank(ArrayBWPMuendlich, 'm', "BWP");
            }
            if (ArrayEnglischSchriftlich != null)
            {
                NotenSpeichernDatenbank(ArrayEnglischSchriftlich, 's', "Englisch");
            }
            if (ArrayEnglischMuendlich != null)
            {
                NotenSpeichernDatenbank(ArrayEnglischMuendlich, 'm', "Englisch");
            }
            if (ArraySportSchriftlich != null)
            {
                NotenSpeichernDatenbank(ArraySportSchriftlich, 's', "Sport");
            }
            if (ArraySportMuendlich != null)
            {
                NotenSpeichernDatenbank(ArraySportMuendlich, 'm', "Sport");
            }
            if (ArrayDeutschSchriftlich != null)
            {
                NotenSpeichernDatenbank(ArrayDeutschSchriftlich, 's', "Deutsch");
            }
            if (ArrayDeutschMuendlich != null)
            {
                NotenSpeichernDatenbank(ArrayDeutschMuendlich, 'm', "Deutsch");
            }
            if (ArrayReligionSchriftlich != null)
            {
                NotenSpeichernDatenbank(ArrayReligionSchriftlich, 's', "Religion");
            }
            if (ArrayReligionMuendlich != null)
            {
                NotenSpeichernDatenbank(ArrayReligionMuendlich, 'm', "Religion");
            }
            if (ArraySozialkundeSchriftlich != null)
            {
                NotenSpeichernDatenbank(ArraySozialkundeSchriftlich, 's', "Sozialkunde");
            }
            if (ArraySozialkundeMuendlich != null)
            {
                NotenSpeichernDatenbank(ArraySozialkundeMuendlich, 'm', "Sozialkunde");
            }
        }

        // Noten in Datenbank speichern mit stored procedure
        static void NotenSpeichernDatenbankSP(int[] ArrayNoten, char cNoteArt, string sFach)
        {
            string sConnectionString = "Host=localhost;Username=postgres;Password=postgres;Database=postgres";

            using (var Connection = new NpgsqlConnection(sConnectionString))
            {
                Connection.Open();
                // Prozedur aufrufen
                using (var cmd = new NpgsqlCommand("Call notenverwaltung.insert_noten('AWP','s',2,'3,1')", Connection)) 
                using (var reader = cmd.ExecuteReader()) ;

            }

        }

        // Noten in Datenbank speichern mit SQL Query
        static void NotenSpeichernDatenbank(int[] ArrayNoten, char cNoteArt, string sFach)
        {
            string sConnectionString = "Host=localhost;Username=postgres;Password=postgres;Database=postgres";
            int iArrayLaenge = ArrayNoten.Length;
            string sNoten = "";
            int i = 0;
            string sDatum = "";

            // Noten als String verketten
            do
            {
                sNoten = sNoten + Convert.ToString(ArrayNoten[i]) + ",";

                i++;
            } while (i < iArrayLaenge);

            // Aktuelles Datum
            sDatum = Convert.ToString(DateTime.Now);

            using (var Connection = new NpgsqlConnection(sConnectionString))
            {
                Connection.Open();

                using (var Command = new NpgsqlCommand())
                {
                    Command.Connection = Connection;
                    Command.CommandText = "INSERT INTO notenverwaltung.noten(fach, notenart, anzahl_noten, noten, datum) VALUES (@p1, @p2, @p3, @p4, '" + sDatum + "')";
                    // Werte einsetzen
                    Command.Parameters.AddWithValue("p1", sFach);
                    Command.Parameters.AddWithValue("p2", cNoteArt);
                    Command.Parameters.AddWithValue("p3", iArrayLaenge);
                    Command.Parameters.AddWithValue("p4", sNoten);
                    Command.ExecuteNonQuery();
                    
                }

            }
        }

        // Stundenplan in .txt Datei schreiben
        static void StundenplanTextdatei()
        {
            string sStundenplan = "";
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"H:\AWP\ProjektNeu\Stundenplan.txt"))
            {
                file.WriteLine("\t** Aktueller Stundenplan **");
                file.WriteLine("\tMo\tDi\tMi\tDo\tFr");
                file.WriteLine("\t------------------------------------");

                for (int j = 0; j < 10; j++)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        sStundenplan = sStundenplan + "\t" + ArrayStundenplan[i, j];
                    }

                    file.WriteLine(sStundenplan);
                    sStundenplan = "";
                }
            }
            // Drucken?

        }

        // Stundenplan bearbeiten
        static void StundenplanBearbeiten()
        {
            int iTag = 0;
            int iStunde = 0;
            string sFach = "";

            Console.WriteLine("** Stunde eintragen **");
            Console.WriteLine("Tage 1=Mo 2=Di 3=Mi 4=Do 5=Fr");
            Console.WriteLine("Stunden 1 -10");
            Console.WriteLine("Welche Stunde soll eingetragen werden?");
            // Einlesen
            Console.WriteLine("Tag:");
            iTag = Convert.ToInt32(Console.ReadLine()) - 1;
            Console.WriteLine("Stunde:");
            iStunde = Convert.ToInt32(Console.ReadLine()) - 1;
            Console.WriteLine("Fach:");
            sFach = Console.ReadLine();

            // Änderung
            ArrayStundenplan[iTag, iStunde] = sFach;
            //Console.WriteLine(ArrayStundenplan[iTag, iStunde]);

            Console.ReadLine();
        }

        // Stundenplan erstellen
        static void StundenplanErstellen()
        {
            ArrayStundenplan = new string[,] { { "", "", "BWP", "D", "E", "", "SP", "SP", "Sk", "Sk" }, { "VS", "VS", "BWP", "AWP", "AWP", "AWP", "", "VSP", "VSP", "VSP" }, { "VSP", "VSP", "VSP", "AWP", "AWP", "AWP", "", "ITS", "ITS", "D" }, { "ITS", "ITS", "VS", "ITSP", "ITSP", "ITSP", "", "BWP", "BWP", "BWP" }, { "E", "E", "Rel", "Rel", "D", "Sk", "", "", "", "" } };
        }

        // Methode zum Ausgeben des Stundenplans
        static void StundenplanAusgeben()
        {
            string sAusgabe = "";
            Console.WriteLine("\t** Aktueller Stundenplan **");
            Console.WriteLine("\tMo\tDi\tMi\tDo\tFr");
            Console.WriteLine("\t------------------------------------");

            for (int j = 0; j < 10; j++)
            {
                for (int i = 0; i < 5; i++)
                {
                    sAusgabe = sAusgabe + "\t" + ArrayStundenplan[i, j];
                }
                Console.WriteLine(sAusgabe);
                sAusgabe = "";
            }

            Console.ReadLine();
        }

        // Methode zum Berechnen des Durchschnitts und anschließender Ausgabe
        // Nimmt die Arrays für die schriftlichen und muendlichen Noten
        static void DurchschnittBerechnen(int[] ArraySchriftlich, int[] ArrayMuendlich)
        {
            double dDurchschnittsNote = 0.0;
            int iZeugnisNote = 0;
            // Test ob die Arrays existieren
            if (ArraySchriftlich == null)
            {
                Console.WriteLine("Das Array für die schriftlichen Noten existiert noch nicht. Bitte vorher über dem Hauptmenü erstellen.");
                Console.ReadLine();
                return;
            }
            if (ArrayMuendlich == null)
            {
                Console.WriteLine("Das Array für die mündlichen Noten existiert noch nicht. Bitte vorher über dem Hauptmenü erstellen.");
                Console.ReadLine();
                return;
            }

            // Die Größe der Arrays bestimmen
            int iAnzahlSchriftlicherNoten = ArraySchriftlich.GetLength(0);
            int iAnzahlMuendlicherNoten = ArrayMuendlich.GetLength(0);

            // Aufsummieren der Noten
            double dSummeSchriftlicheNoten = ArrayAufsummieren(ArraySchriftlich, iAnzahlSchriftlicherNoten);
            double dSummeMuendlicheNoten = ArrayAufsummieren(ArrayMuendlich, iAnzahlMuendlicherNoten);

            // Ausgabe des Durchschnitts und Zeugnisnote
            if (iAnzahlMuendlicherNoten >= (iAnzahlSchriftlicherNoten * 2))
            {
                dDurchschnittsNote = (((dSummeSchriftlicheNoten / iAnzahlSchriftlicherNoten) + (dSummeMuendlicheNoten / iAnzahlMuendlicherNoten)) / 2);
                iZeugnisNote = KonvertiereDurchschnittInZeugnisnote(dDurchschnittsNote);
                Console.WriteLine("Notendurchschnitt: " + dDurchschnittsNote + "; Zeugnisnote: " + iZeugnisNote);
                Console.ReadLine();
            }
            else
            {
                dDurchschnittsNote = ((((dSummeSchriftlicheNoten / iAnzahlSchriftlicherNoten) * 2) + (dSummeMuendlicheNoten / iAnzahlMuendlicherNoten)) / 3);
                iZeugnisNote = KonvertiereDurchschnittInZeugnisnote(dDurchschnittsNote);
                Console.WriteLine("Notendurchschnitt: " + dDurchschnittsNote + "; Zeugnisnote: " + iZeugnisNote);
                Console.ReadLine();
            }
        }

        // Methode um der übergebenen Durchschnittsnote eine Zeugnisnote zuzuordnen
        // Übergeben wird die Durchschnittsnote
        // Die Zeugnisnote wird zurückgegeben
        static int KonvertiereDurchschnittInZeugnisnote(double dDurchschnitt)
        {
            int iNote = 0;
            // Alle Notenbereiche durchlaufen
            if (dDurchschnitt >= 1.0 & dDurchschnitt < 1.5)
            {
                iNote = 1;
            }
            else if (dDurchschnitt >= 1.5 & dDurchschnitt < 2.5)
            {
                iNote = 2;
            }
            else if (dDurchschnitt >= 2.5 & dDurchschnitt < 3.5)
            {
                iNote = 3;
            }
            else if (dDurchschnitt >= 3.5 & dDurchschnitt <= 4.0)
            {
                iNote = 4;
            }
            else if (dDurchschnitt > 4.0 & dDurchschnitt <= 5.0)
            {
                iNote = 5;
            }
            else if (dDurchschnitt > 5.0 & dDurchschnitt <= 6.0)
            {
                iNote = 6;
            }
            else
            {
                Console.WriteLine("Notendurchschnitt ist außerhalb eines sinnvollen Bereiches, Bitte die Noten überprüfen.");
            }
            return iNote;
        }

        // Die Noten im Array werden aufsummiert
        // Übergeben wird das Array mit den Noten und die Anzahl der Element = Noten
        // Die Summe wird wieder zurückgegeben
        static double ArrayAufsummieren(int[] ArrayNoten, int iAnzahlNoten)
        {
            double dSumme = 0.0;
            for (int i = 0; i < iAnzahlNoten; i++)
            {
                dSumme = dSumme + ArrayNoten[i];
            }
            return dSumme;
        }

        // Methode zum ändern des Arrays 
        // Übergeben wird das zu ändernde Array
        static void ArrayAendern(int[] Array)
        {
            int iNote = 0;
            Console.WriteLine("Welche Position soll geändert werden?");
            int k = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Neue Note eingeben:");
            do
            {
                iNote = Convert.ToInt32(Console.ReadLine());
            } while (!TestEingegebeneNote(iNote));
            Array[k] = iNote;

            Console.WriteLine("Die Note wurde geändert!");
            Console.ReadLine();
        }

        // Methode zur Ausgabe des Arrays inklusive Position der Noten
        // Nimmt ein Array an
        static void NotenAusgeben(int[] Array)
        {
            Console.WriteLine("Anzahl der Noten: " + Array.Length);
            for (int i = 0; i < Array.Length; i++)
            {
                Console.WriteLine("Note an der Position " + i + ":");
                Console.WriteLine(Array[i]);
            }
            Console.ReadLine();
        }

        // Methode um Menu auf der Konsole auszugeben und Auswahl des Anwenders einzulesen
        static int Hauptmenu()
        {
            Console.Clear();

            int iAuswahl = 0;

            Console.WriteLine("----------------------Menüführung----------------------");
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("<1>: Noteneingabe");
            Console.WriteLine("<2>: Noten ausgeben");
            Console.WriteLine("<3>: Notenschnitt berechnen");
            Console.WriteLine("<4>: Stundenplan ausgeben");
            Console.WriteLine("<5>: Stundenplan bearbeiten");
            Console.WriteLine("<6>: Stundenplan in Textdatei schreiben");
            Console.WriteLine("<7>: Noten in Datenbank speichern");
            Console.WriteLine("<0>: Programm beenden\n");
            Console.WriteLine("Bitte geben sie ihre Wahl ein:");

            iAuswahl = Convert.ToInt32(Console.ReadLine());
            return iAuswahl;
        }

        // Methode zum Anzeigen der Fächerauswahl und Einlesen der Auswahl
        // Gibt die Auswahl zurück
        static int Faecher()
        {
            int iFach = 0;
            Console.WriteLine("Fächerauswahl");
            Console.WriteLine("<1>: AWP");
            Console.WriteLine("<2>: ITS");
            Console.WriteLine("<3>: VS");
            Console.WriteLine("<4>: BWP");
            Console.WriteLine("<5>: Englisch");
            Console.WriteLine("<6>: Sport");
            Console.WriteLine("<7>: Deutsch");
            Console.WriteLine("<8>: Religion");
            Console.WriteLine("<9>: Sozialkunde");
            Console.WriteLine("<0>: Zurück zum Hauptmenü");

            iFach = Convert.ToInt32(Console.ReadLine());
            return iFach;
        }

        // Methode um das Array zu erstellen
        // Nimmt die Notenart an um die Frage nach der Notenanzahl zu individualisieren
        // Gibt das Array zurück
        static int[] ArrayErstellen(char cNoteArt)
        {
            int iAnzahlNoten = 0;
            int[] ArrayNoten;
            int iNote = 0;

            if (cNoteArt == 's')
            {
                Console.WriteLine("Wie viele schriftliche Noten sollen eingeben werden?");
            }
            if (cNoteArt == 'm')
            {
                Console.WriteLine("Wie viele mündliche Noten sollen eingegeben werden?");
            }
            iAnzahlNoten = Convert.ToInt32(Console.ReadLine());
            ArrayNoten = new int[iAnzahlNoten];
            // Die Laufvariable wird in der Schleife inkrementiert/dekrementiert um die Konsolenausgabe interaktiver wirken zu lassen
            for (int i = 0; i < iAnzahlNoten; i++)
            {

                Console.WriteLine("Bitte die " + (i + 1) + ". Note eingeben:");

                do
                {
                    iNote = Convert.ToInt32(Console.ReadLine());
                } while (!TestEingegebeneNote(iNote));
                ArrayNoten[i] = iNote;
            }
            Console.WriteLine("Das Array mit den Noten wurde erstellt!");
            return ArrayNoten;
        }

        // Methode zum überprüfen ob die eingegebenen Noten sinnvoll sind
        static Boolean TestEingegebeneNote(int iNote)
        {
            //Array mit erlaubten Noten erstellen
            int[] ArrayErlaubteNoten = new int[6] { 1, 2, 3, 4, 5, 6 };

            Boolean bNotePasst = true;
            if (ArrayErlaubteNoten.Contains(iNote))
            {
                bNotePasst = true;
            }
            else
            {
                Console.WriteLine("Bitte eine Note zwischen 1 und 6 eingeben.");
                bNotePasst = false;
            }
            return bNotePasst;
        }

        // Methode zum Abfragen der Notenart, also Schriftlich oder Mündlich
        // Gibt die Notenart als char zurück
        static char SchriftlichOderMuendlich()
        {
            char cNoteArt;
            Console.WriteLine("Schriftliche (s) oder muendliche (m) Noten?");
            do
            {
                cNoteArt = Convert.ToChar(Console.ReadLine());
            } while (!TestNoteArt(cNoteArt));
            return cNoteArt;
        }

        // Methode zum überprüfen ob ein "s" oder "m" für Notenart eingeben wurde
        static Boolean TestNoteArt(char cNoteArt)
        {
            Boolean bKorrekteEingabe = true;
            if (cNoteArt == 's' | cNoteArt == 'm')
            {
                bKorrekteEingabe = true;
            }
            else
            {
                Console.WriteLine("Bitte 's' oder 'm' eingeben");
                bKorrekteEingabe = false;
            }
            return bKorrekteEingabe;
        }

        // Methode zur Auswahl und anschließendem Erstellen des Arrays
        // Nimmt das Fach und die Notenart an
        static void ArrayErstellenAuswahl(int iFach, char cNoteArt)
        {
            switch (iFach)
            {
                case 1:
                    // AWP
                    //Array erstellen für schriftliche Noten
                    if (cNoteArt == 's')
                    {
                        ArrayAWPSchriftlich = ArrayErstellen(cNoteArt);
                    }
                    // Array erstellen für muendliche Noten
                    if (cNoteArt == 'm')
                    {
                        ArrayAWPMuendlich = ArrayErstellen(cNoteArt);
                    }
                    break;

                case 2:
                    // ITS
                    //Array erstellen für schriftliche Noten
                    if (cNoteArt == 's')
                    {
                        ArrayITSSchriftlich = ArrayErstellen(cNoteArt);
                    }
                    // Array erstellen für muendliche Noten
                    if (cNoteArt == 'm')
                    {
                        ArrayITSMuendlich = ArrayErstellen(cNoteArt);
                    }
                    break;

                case 3:
                    // VS
                    //Array erstellen für schriftliche Noten
                    if (cNoteArt == 's')
                    {
                        ArrayVSSchriftlich = ArrayErstellen(cNoteArt);
                    }
                    // Array erstellen für muendliche Noten
                    if (cNoteArt == 'm')
                    {
                        ArrayVSMuendlich = ArrayErstellen(cNoteArt);
                    }
                    break;

                case 4:
                    // BWP
                    //Array erstellen für schriftliche Noten
                    if (cNoteArt == 's')
                    {
                        ArrayBWPSchriftlich = ArrayErstellen(cNoteArt);
                    }
                    // Array erstellen für muendliche Noten
                    if (cNoteArt == 'm')
                    {
                        ArrayBWPMuendlich = ArrayErstellen(cNoteArt);
                    }
                    break;

                case 5:
                    // Englisch
                    //Array erstellen für schriftliche Noten
                    if (cNoteArt == 's')
                    {
                        ArrayEnglischSchriftlich = ArrayErstellen(cNoteArt);
                    }
                    // Array erstellen für muendliche Noten
                    if (cNoteArt == 'm')
                    {
                        ArrayEnglischMuendlich = ArrayErstellen(cNoteArt);
                    }
                    break;

                case 6:
                    // Sport
                    //Array erstellen für schriftliche Noten
                    if (cNoteArt == 's')
                    {
                        ArraySportSchriftlich = ArrayErstellen(cNoteArt);
                    }
                    // Array erstellen für muendliche Noten
                    if (cNoteArt == 'm')
                    {
                        ArraySportMuendlich = ArrayErstellen(cNoteArt);
                    }
                    break;

                case 7:
                    // Deutsch
                    //Array erstellen für schriftliche Noten
                    if (cNoteArt == 's')
                    {
                        ArrayDeutschSchriftlich = ArrayErstellen(cNoteArt);
                    }
                    // Array erstellen für muendliche Noten
                    if (cNoteArt == 'm')
                    {
                        ArrayDeutschMuendlich = ArrayErstellen(cNoteArt);
                    }
                    break;

                case 8:
                    // Religion
                    //Array erstellen für schriftliche Noten
                    if (cNoteArt == 's')
                    {
                        ArrayReligionSchriftlich = ArrayErstellen(cNoteArt);
                    }
                    // Array erstellen für muendliche Noten
                    if (cNoteArt == 'm')
                    {
                        ArrayReligionMuendlich = ArrayErstellen(cNoteArt);
                    }
                    break;

                case 9:
                    // Sozialkunde
                    //Array erstellen für schriftliche Noten
                    if (cNoteArt == 's')
                    {
                        ArraySozialkundeSchriftlich = ArrayErstellen(cNoteArt);
                    }
                    // Array erstellen für muendliche Noten
                    if (cNoteArt == 'm')
                    {
                        ArraySozialkundeMuendlich = ArrayErstellen(cNoteArt);
                    }
                    break;
            }
        }

        // Methode zum überprüfen ob wirklich ein j oder n eingeben wurde
        static Boolean TestJaNein(char cAntwort)
        {
            Boolean bKorrekteEingabe = true;
            if (cAntwort == 'j' | cAntwort == 'n')
            {
                bKorrekteEingabe = true;
            }
            else
            {
                Console.WriteLine("Bitte 'j' oder 'n' eingeben.");
                bKorrekteEingabe = false;
            }
            return bKorrekteEingabe;
        }

        // Methode zum überprüfen ob ein Array schon existiert und bei Bedarf die Möglichkeit zum Erstellen gibt
        // Nimmt das Array, Fach und Notenart
        static Boolean TestArrayExistiert(int[] Array, int iFach, char cNoteArt)
        {
            Boolean bArrayExistiert = true;
            if (Array == null)
            {
                bArrayExistiert = false;
                char cArrayJetztErstellen = ' ';
                Console.WriteLine("Das Array wurde noch nicht erstellt. Möchten Sie es jetzt erstellen? (j/n)");
                do
                {
                    cArrayJetztErstellen = Convert.ToChar(Console.ReadLine());
                } while (!TestJaNein(cArrayJetztErstellen));

                if (cArrayJetztErstellen == 'j')
                {
                    ArrayErstellenAuswahl(iFach, cNoteArt);
                    Console.WriteLine("Das Array wurde jetzt erstellt. Sie werden wieder zum vorherigen Menü weitergeleitet.");
                    Console.ReadLine();
                }
            }
            return bArrayExistiert;
        }

        // Methode zur Ausgabe, Änderung und Erstellen des Arrays
        static void ArrayAusgabeAendern(int iFach, char cNoteArt, int[] ArraySchriftlich, int[] ArrayMuendlich)
        {
            char cAntwort = ' ';
            if (cNoteArt == 's')
            {
                // Test ob Array existiert und bei Bedarf erstellen
                if (TestArrayExistiert(ArraySchriftlich, iFach, cNoteArt))
                {
                    NotenAusgeben(ArraySchriftlich);
                }
            }
            if (cNoteArt == 'm')
            {
                if (TestArrayExistiert(ArrayMuendlich, iFach, cNoteArt))
                {
                    NotenAusgeben(ArrayMuendlich);
                }
            }

            // Sollen die Noten geändert werden?
            Console.WriteLine("Sollen die Noten geändert werden? (j/n)");
            // Test ob j oder n eingegeben wurde
            do
            {
                cAntwort = Convert.ToChar(Console.ReadLine());
            } while (!TestJaNein(cAntwort));

            if (cAntwort == 'j')
            {
                Console.WriteLine("Sollen die schriftlichen Noten geändert werden? (j/n)");
                do
                {
                    cAntwort = Convert.ToChar(Console.ReadLine());
                } while (!TestJaNein(cAntwort));

                if (cAntwort == 'j')
                {
                    cNoteArt = 's';
                    // Test ob Array existiert und bei Bedarf erstellen
                    if (TestArrayExistiert(ArraySchriftlich, iFach, cNoteArt))
                    {
                        ArrayAendern(ArraySchriftlich);
                    }
                }
                Console.WriteLine("Sollen die muendlichen Noten geändert werden? (j/n)");
                do
                {
                    cAntwort = Convert.ToChar(Console.ReadLine());
                } while (!TestJaNein(cAntwort));

                if (cAntwort == 'j')
                {
                    cNoteArt = 'm';
                    // Test ob Array existiert und bei Bedarf erstellen
                    if (TestArrayExistiert(ArrayMuendlich, iFach, cNoteArt))
                    {
                        ArrayAendern(ArrayMuendlich);
                    }
                }
            }
        }

        // Methode zur Auswahl und Ausgabe des Arrays 
        // Nimmt das Fach und die Notenart
        static void ArrayAusgebenUndAendern(int iFach, char cNoteArt)
        {
            switch (iFach)
            {
                case 1:
                    ArrayAusgabeAendern(iFach, cNoteArt, ArrayAWPSchriftlich, ArrayAWPMuendlich);
                    break;

                case 2:
                    ArrayAusgabeAendern(iFach, cNoteArt, ArrayITSSchriftlich, ArrayITSMuendlich);
                    break;

                case 3:
                    ArrayAusgabeAendern(iFach, cNoteArt, ArrayVSSchriftlich, ArrayVSMuendlich);
                    break;

                case 4:
                    ArrayAusgabeAendern(iFach, cNoteArt, ArrayBWPSchriftlich, ArrayBWPMuendlich);
                    break;

                case 5:
                    ArrayAusgabeAendern(iFach, cNoteArt, ArrayEnglischSchriftlich, ArrayEnglischMuendlich);
                    break;

                case 6:
                    ArrayAusgabeAendern(iFach, cNoteArt, ArraySportSchriftlich, ArraySportMuendlich);
                    break;

                case 7:
                    ArrayAusgabeAendern(iFach, cNoteArt, ArrayDeutschSchriftlich, ArrayDeutschMuendlich);
                    break;

                case 8:
                    ArrayAusgabeAendern(iFach, cNoteArt, ArrayReligionSchriftlich, ArrayReligionMuendlich);
                    break;

                case 9:
                    ArrayAusgabeAendern(iFach, cNoteArt, ArraySozialkundeSchriftlich, ArraySozialkundeMuendlich);
                    break;
            }
        }

        // Methode zur Auswahl des Faches zur Durchschnittsberechnung
        // Nimmt das Fach
        static void AuswahlDurchschnittBerechnen(int iFach)
        {
            switch (iFach)
            {
                // AWP
                case 1:
                    DurchschnittBerechnen(ArrayAWPSchriftlich, ArrayAWPMuendlich);
                    break;
                // ITS
                case 2:
                    DurchschnittBerechnen(ArrayITSSchriftlich, ArrayITSMuendlich);
                    break;
                // VS
                case 3:
                    DurchschnittBerechnen(ArrayVSSchriftlich, ArrayITSMuendlich);
                    break;
                // BWP
                case 4:
                    DurchschnittBerechnen(ArrayBWPSchriftlich, ArrayBWPMuendlich);
                    break;
                // Englisch
                case 5:
                    DurchschnittBerechnen(ArrayEnglischSchriftlich, ArrayEnglischMuendlich);
                    break;
                // Sport
                case 6:
                    DurchschnittBerechnen(ArraySportSchriftlich, ArraySportMuendlich);
                    break;
                // Deutsch
                case 7:
                    DurchschnittBerechnen(ArrayDeutschSchriftlich, ArrayDeutschMuendlich);
                    break;
                // Religion
                case 8:
                    DurchschnittBerechnen(ArrayReligionSchriftlich, ArrayReligionMuendlich);
                    break;
                // Sozialkunde
                case 9:
                    DurchschnittBerechnen(ArraySozialkundeSchriftlich, ArraySozialkundeMuendlich);
                    break;
            }
        }
    }
}
