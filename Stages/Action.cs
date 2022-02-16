using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Reflection;

namespace Stages
{
    abstract class ConstructDocument
    {
        public void Run(StagesList stages = null)
        {
            Header();
            Body(stages);
        }
        protected abstract void Header();
        protected abstract void Body(StagesList stages);
    }
    class MoveDocument : ConstructDocument
    {
        ConsoleMove consoleMove = new ConsoleMove();
        protected override void Header()
        {
            consoleMove.Print("Процесс согласования документа\n", Console.WindowWidth / 3);
            consoleMove.Print("Введите 'y(yes)' если согласовано, либо 'n(no)' если не согласовано\n");
        }
        protected override void Body(StagesList stages)
        {
            SaveModel saveModel = new SaveModel();
            List<MoveStages> moveStagesList = new List<MoveStages>();

            for(int i = 0; i < stages.Stages.Count; i++)
            {
                MoveStages moveStages = new MoveStages();

                consoleMove.Print();
                consoleMove.Print("Наименование этапа: " + stages.Stages[i].Name);
                moveStages.Name = stages.Stages[i].Name;
                consoleMove.Print("Согласующий: " + stages.Stages[i].Performer);
                moveStages.Performer = stages.Stages[i].Performer;
                moveStages.Decision = consoleMove.GetInputData("Согласовано: ");
                moveStages.Comment = consoleMove.GetInputData("Комментарий: ");
                moveStagesList.Add(moveStages);
            }
            saveModel.Launch(moveStagesList);

            string path = Path.GetFullPath(@"..\..\..\" + @$"\test_res.json");
            string json = JsonSerializer.Serialize<List<MoveStages>>(moveStagesList);

            using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
            {
                sw.WriteLine(json);
            }
        }
    }
    class Report : ConstructDocument
    {
        ConsoleMove consoleMove = new ConsoleMove();
        protected override void Header()
        {
            consoleMove.Print();
            consoleMove.GetInputData("\nНажмите любую клавишу, чтобы открыть отчёт");

            Console.Clear();
            consoleMove.Print("Таблица согласования", Console.WindowWidth / 3);
            consoleMove.Print();
        }
        protected override void Body(StagesList stages)
        {
            SaveModel saveModel = new SaveModel();
            List<MoveStages> moveStagesList = new List<MoveStages>();

            saveModel.viewModel = ViewModel.getModel();
            moveStagesList = saveModel.viewModel.MoveStagesList;
            moveStagesList.Insert(0, new MoveStages { Name = "Наименование этапа", Performer = "Согласующий", Decision = "Результат", Comment = "Комментарий" });

            Console.Write("|\t");
            Console.Write(moveStagesList[0].Name);
            Console.Write("\t|\t");
            Console.Write(moveStagesList[0].Performer);
            Console.Write("\t|\t");
            Console.Write(moveStagesList[0].Decision);
            Console.Write("\t|\t");
            Console.Write(moveStagesList[0].Comment);
            Console.Write("\t|");
            Console.WriteLine();
            consoleMove.Print();

            for (int i = 1; i < moveStagesList.Count; i++)
            {
                Console.Write("|");
                Console.Write(moveStagesList[i].Name);
                Console.Write("\t|");
                Console.Write(moveStagesList[i].Performer);
                Console.Write("\t|");

                if (moveStagesList[i].Decision.ToLower() == "y" || moveStagesList[i].Decision.ToLower() == "yes")
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Согласовано");
                    Console.ResetColor();
                }
                else if (moveStagesList[i].Decision.ToLower() == "n" || moveStagesList[i].Decision.ToLower() == "no")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Не согласовано");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("Пропущено");
                    Console.ResetColor();
                }

                Console.Write("\t|");
                Console.Write(moveStagesList[i].Comment);
                Console.Write("|");
                Console.WriteLine();
                consoleMove.Print();
            }
        }
    }
    class SaveModel
    {
        public ViewModel viewModel { get; set; }
        public void Launch(List<MoveStages> moveStagesList)
        {
            viewModel = ViewModel.getModel(moveStagesList);
        }
    }
    class ViewModel
    {
        private static ViewModel Model;

        public List<MoveStages> MoveStagesList { get; private set; }

        protected ViewModel(List<MoveStages> moveStagesList)
        {
            this.MoveStagesList = moveStagesList;
        }

        public static ViewModel getModel(List<MoveStages> moveStagesList = null)
        {
            if (Model == null)
                Model = new ViewModel(moveStagesList);
            return Model;
        }
    }
    class ConsoleMove
    {
        public void Print()
        {
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.Write("-");
            }
        }
        public void Print(string text)
        {
            Console.WriteLine(text);
        }
        public void Print(string text, int width = 0, int height = 0)
        {
            Console.SetCursorPosition(width, height);
            Console.WriteLine(text);
        }
        public string GetInputData(string text)
        {
            Console.Write(text);

            return Console.ReadLine();
        }
    }
}
