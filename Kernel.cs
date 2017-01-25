using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Sys = Cosmos.System;
namespace CosmosKernel4
{
    public class Kernel : Sys.Kernel
    {

        string current_directory = "0:\\";
        int counter = 0;
        List<string> batFiles = new List<string>();
        List<string> batFilesList = new List<string>();
        List<string> variables = new List<string>();
        List<int> variableVal = new List<int>();
        List<Variable> varList = new List<Variable>();
        List<File> files = new List<File>();

        protected override void BeforeRun()
        {
            var fs = new Sys.FileSystem.CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);


            Console.WriteLine("Cosmos booted successfully. Type help to view functions");
        }

        protected override void Run()
        {
            Console.Write(current_directory);

            var input = Console.ReadLine();
            interpretCMD(input);
        }


        //**************************************** INTERPRET CMDS ****************************************
        public void interpretCMD(string input)
        {
            string[] split = input.Split(' ');
            String command = split[0];
            int CMD = command.ToCharArray().Length;
            if (input.ToLower() == "help")
                helpCMD();
            else if (CMD == 3)
            {
                if (input.ToLower() == "dir")
                    dirCMD();
                if (input.Substring(0, 3) == "set")
                    setCMD(input);
                if (input.Substring(0, 3) == "run")
                    runCMD(input);
                if (input.Substring(0, 3) == "add")
                {
                    operations(input);
                }
                if (input.Substring(0, 3) == "sub")
                {
                    operations(input);
                }
                if (input.Substring(0, 3) == "mul")
                {
                    operations(input);
                }
                if (input.Substring(0, 3) == "div")
                {
                    operations(input);
                }


            }
            if (CMD == 4)
            {
                if (input.ToLower().Substring(0, 4) == "echo")
                    echoCMD(input);
            }
            if (CMD == 5)
            {
                if (input.ToLower().Substring(0, 5) == "clear")
                    clearCMD();
            }
            if (CMD == 6)
            {
                if (input.ToLower().Substring(0, 6) == "create")
                    createCMD(input);
                if (input.ToLower().Substring(0, 6) == "runall")
                    runallCMD(input);
            }
        }

        //**************************************** HELP ****************************************

        private void helpCMD()
        {
            Console.WriteLine("clear\tType clear to clear console");
            Console.WriteLine("echo\tType echo and your text after to repeat an input");
            Console.WriteLine("create\tCreate a file by typing create <filename>.<extention>");
            Console.WriteLine("dir\tView the directory");
            Console.WriteLine("set\tSet variables for an expression by typing set <var> = <#>");
            Console.WriteLine("add\tadd variables for an expression by typing add <var> = <#> + <#>");
            Console.WriteLine("sub\tsubstract variables for an expression by typing sub <var> = <#> - <#>");
            Console.WriteLine("mul\tmultiply variables for an expression by typing mul <var> = <#> * <#>");
            Console.WriteLine("div\tdivide variables for an expression by typing div <var> = <#> / <#>");
            Console.WriteLine("run\tRun a file by typing run <#> <file.bat>");
            Console.WriteLine("runall\tRun more than one file by typing runall <#> <file.bat> <file.bat>");

        }
        private void clearCMD()
        {
            Console.Clear();
        }

        //**************************************** ECHO ****************************************

        private void echoCMD(String input)
        {
            bool print = false;
            for (int i = 0; i < variables.Count; i++)
            {
                if (input.Substring(5) == variables[i])
                {
                    Console.WriteLine(variableVal[i]);
                    print = true;

                }
            }
            if (!print)
            {
                Console.WriteLine(input.Substring(5));
            }
        }


        //**************************************** CREATE ****************************************

        private void createCMD(string input)
        {
            List<string> fileData = new List<string>();

            string fs = "";
            int index = input.IndexOf('.');
            string fileName = input.Substring(7, index - 7);
            int fileSize;
            File newFile = new File(fileName);

            string extension = input.Substring(index);
            string text = Console.ReadLine();

            while (text != "save")
            {

                fileData.Add(text);
                fs += text;
                text = Console.ReadLine();
            }

            fileSize = fs.Length;
            newFile.setFileData(fileData);
            newFile.setFileExt(extension);
            newFile.setFileSize(fileSize);
            files.Add(newFile);
            Console.WriteLine("File has been saved.");
        }

        //**************************************** DIRECTORY ****************************************

        public void dirCMD()
        {
            for (int i = 0; i < files.Count; i++)
            {
                Console.WriteLine(files[i].getFileName() + files[i].getExt() + "\t" + files[i].getSize() + " byte(s)");
            }
        }

        //**************************************** SET ****************************************       

        private void setCMD(string input)
        {
            string varName = input.Substring(4, 1);
            string varValString = input.Substring(6);
            int varVal = Int32.Parse(varValString);
            try
            {
                for (int i = 0; i < variables.Count; i++)
                {

                    if (varName == variables[i])
                    {
                        variables.RemoveAt(i);
                        variableVal.RemoveAt(i);
                        break;
                    }

                }
                Variable newVar = new Variable(varName, varVal);
                newVar.setVal(varVal);
                newVar.setVar(varName);
                variables.Add(newVar.getVar());
                variableVal.Add(newVar.getVal());
            }
            catch(Exception ex)
            {
                Console.WriteLine("Messed up, try again.");
            }
        }
        //**************************************** OPERATIONS **************************************** 
        private void operations(string input)
        {
            string operation = input.Substring(0, 3);
            bool noVar1 = true;
            bool noVar2 = true;
            int val1 = 0;
            int val2 = 0;
            string[] operations = input.Substring(4, input.Length - 4).Split(' ');
            string varName;
            if (operations.Length == 3)
            {
                varName = operations[2];
            }
            else
                varName = operations[1];

            int varVal = 0;
            try
            {
                switch (operation)
                {
                    case "add":

                        for (int i = 0; i < variables.Count; i++)
                        {
                            if (operations[0] == variables[i])
                            {
                                varVal = variableVal[i] + Int32.Parse(operations[1]);

                                noVar1 = false;
                                break;

                            }
                            if (operations[1] == variables[i])
                            {
                                varVal = variableVal[i] + Int32.Parse(operations[0]);

                                noVar2 = false;
                                break;

                            }

                        }
                        for (int i = 0; i < variables.Count; i++)
                        {
                            if (operations[0] == variables[i])
                            {
                                val1 = variableVal[i];
                                noVar1 = false;

                            }
                        }
                        for (int i = 0; i < variables.Count; i++)
                        {
                            if (operations[1] == variables[i])
                            {
                                val2 = variableVal[i];
                                noVar2 = false;

                            }
                        }
                        if (operations.Length == 3 && noVar1 == false && noVar2 == false)
                        {
                            varVal = val1 + val2;
                        }

                        if (operations.Length < 3)
                            for (int i = 0; i < variables.Count; i++)
                            {
                                if (variables[i] == varName)
                                {
                                    varVal = variableVal[i] + Int32.Parse(operations[0]);
                                    variables.RemoveAt(i);
                                    variableVal.RemoveAt(i);
                                }
                            }
                        else if (operations.Length == 3 && noVar1 == true && noVar2 == true)
                            varVal = Int32.Parse(operations[0]) + Int32.Parse(operations[1]);

                        break;
                    case "sub":

                        for (int i = 0; i < variables.Count; i++)
                        {
                            if (operations[0] == variables[i])
                            {
                                varVal = variableVal[i] - Int32.Parse(operations[1]);
                                noVar1 = false;
                                break;

                            }
                            if (operations[1] == variables[i])
                            {
                                varVal = Int32.Parse(operations[0]) - variableVal[i];
                                noVar2 = false;
                                break;

                            }
                        }
                        if (operations.Length < 3)
                            for (int i = 0; i < variables.Count; i++)
                            {
                                if (variables[i] == varName)
                                {
                                    varVal = variableVal[i] - Int32.Parse(operations[0]);
                                    variables.RemoveAt(i);
                                    variableVal.RemoveAt(i);

                                }
                            }
                        else if (operations.Length == 3 && noVar1 == true && noVar2 == true)
                            varVal = Int32.Parse(operations[0]) - Int32.Parse(operations[1]);

                        break;
                    case "mul":
                        for (int i = 0; i < variables.Count; i++)
                        {
                            if (operations[0] == variables[i])
                            {
                                varVal = variableVal[i] * Int32.Parse(operations[1]);
                                noVar1 = false;
                                break;

                            }
                            if (operations[1] == variables[i])
                            {
                                varVal = Int32.Parse(operations[0]) * variableVal[i];
                                noVar2 = false;
                                break;

                            }
                        }
                        if (operations.Length < 3)
                            for (int i = 0; i < variables.Count; i++)
                            {
                                if (variables[i] == varName)
                                {
                                    varVal = variableVal[i] * Int32.Parse(operations[0]);
                                    variables.RemoveAt(i);
                                    variableVal.RemoveAt(i);

                                }
                            }
                        else if (operations.Length == 3 && noVar1 == true && noVar2 == true)
                            varVal = Int32.Parse(operations[0]) * Int32.Parse(operations[1]);

                        break;
                    case "div":
                        for (int i = 0; i < variables.Count; i++)
                        {
                            if (operations[0] == variables[i])
                            {
                                varVal = variableVal[i] / Int32.Parse(operations[1]);
                                noVar1 = false;
                                break;

                            }
                            if (operations[1] == variables[i])
                            {
                                varVal = Int32.Parse(operations[0]) / variableVal[i];
                                noVar2 = false;
                                break;

                            }
                        }
                        if (operations.Length < 3)
                            for (int i = 0; i < variables.Count; i++)
                            {
                                if (variables[i] == varName)
                                {
                                    varVal = variableVal[i] / Int32.Parse(operations[0]);
                                    variables.RemoveAt(i);
                                    variableVal.RemoveAt(i);

                                }
                            }
                        else if (operations.Length == 3 && noVar1 == true && noVar2 == true)
                            varVal = Int32.Parse(operations[0]) / Int32.Parse(operations[1]);

                        break;
                    default:
                        break;
                }
                Variable newVar = new Variable(varName, varVal);
                newVar.setVal(varVal);
                newVar.setVar(varName);
                variables.Add(newVar.getVar());
                variableVal.Add(newVar.getVal());
            }
            catch (Exception ex)
            {

                Console.WriteLine("fucked up");
            }
        }
        //**************************************** RUN **************************************** 
        private void runCMD(string input)
        {
            try
            {
            int index = input.IndexOf('.');
            string filename = input.Substring(6, index - 6);
            string temp = input.Substring(4, 1);
            int runAm = Int32.Parse(temp);
            string ext = input.Substring(index);

                Console.WriteLine("Running " + runAm + " time(s)");
                for (int n = 0; n < runAm; n++)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        if (files[i].getFileName() == filename && files[i].getExt() == ext && ext == ".bat")
                        {
                            Console.WriteLine("Reading data...");
                            batFiles = files[i].getFileData();
                        }
                    }
                    for (int j = 0; j < batFiles.Count; j++)
                    {
                        interpretCMD(batFiles[j]);
                    }
                }
                Console.WriteLine("Done deal");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Probably for that number in front again... Try again.");
            }
        }





        //**************************************** RUN ALL **************************************** 
        private void runallCMD(string input)
        {
            string filename = "";
            string ext = "";

            string temp = input.Substring(7, 1);
            int runAm = Int32.Parse(temp);
            int inputLength = input.Length;
            string batchstring = input.Substring(9, input.Length - 9);
            string[] batcharray = batchstring.Split(' ');

            try
            {
                for (int n = 0; n < runAm; n++)
                {
                    for (int i = 0; i < batcharray.Length; i++)
                    {
                        int index = batcharray[i].IndexOf('.');
                        filename = batcharray[i].Substring(0, batcharray[i].Length - 4);
                        ext = batcharray[i].Substring(index, 4);
                        runall2(batcharray, index, filename, ext);

                    }
                }
                Console.WriteLine("Done deal");

            }

            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong, try again");
            }
        }
       

        private void runall2(string[] batcharray, int index, string filename, string ext)
        {
          try
          {
                for (int i = 0; i < files.Count; i++)
                {
                    if (files[i].getFileName() == filename && files[i].getExt() == ext && ext == ".bat")
                    {

                        batFilesList = files[i].getFileData();
                    }

                }
                for (int j = 0; j < batFilesList.Count; j++)
                {
                    interpretCMD(batFilesList[j]);
                }

         }
        catch (Exception ex)
        {
            Console.WriteLine("Something went wrong, try again");
        }

       }
    }
}
