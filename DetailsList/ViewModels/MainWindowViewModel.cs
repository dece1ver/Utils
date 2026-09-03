using DetailsList.Infrastructure;
using DetailsList.Infrastructure.Commands;
using DetailsList.Infrastructure.Database;
using DetailsList.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace DetailsList.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        Thread getFilesThread;

        private string _Status = string.Empty;
        /// <summary>
        /// Статус
        /// </summary>
        public string Status
        {
            get => _Status;
            set => Set(ref _Status, value);
        }

        private bool _ModeComboboxEnabled = true;

        public bool ModeComboboxEnabled
        {
            get => _ModeComboboxEnabled;
            set => Set(ref _ModeComboboxEnabled, value);
        }


        private bool _BrowseButtonEnabled = true;

        public bool BrowseButtonEnabled
        {
            get => _BrowseButtonEnabled;
            set => Set(ref _BrowseButtonEnabled, value);
        }

        private bool _SaveButtonEnabled;

        public bool SaveButtonEnabled
        {
            get => _SaveButtonEnabled;
            set => Set(ref _SaveButtonEnabled, value);
        }

        private bool _FindButtonEnabled;

        public bool FindButtonEnabled
        {
            get => _FindButtonEnabled;
            set => Set(ref _FindButtonEnabled, value);
        }

        private string _FindButtonText = "Сформировать";

        public string FindButtonText
        {
            get => _FindButtonText;
            set => Set(ref _FindButtonText, value);
        }


        private bool _GetFilesThreadFlag = false;

        public bool GetFilesThreadFlag
        {
            get => _GetFilesThreadFlag;
            set => Set(ref _GetFilesThreadFlag, value);
        }

        private double _Progress;
        /// <summary>
        /// Значение прогрессбара
        /// </summary>
        public double Progress
        {
            get => _Progress;
            set => Set(ref _Progress, value);
        }

        private double _ProgressMaxValue;
        /// <summary>
        /// Максимальное значение прогрессбара
        /// </summary>
        public double ProgressMaxValue
        {
            get => _ProgressMaxValue;
            set => Set(ref _ProgressMaxValue, value);
        }

        private Visibility _ProgressBarVisibility = Visibility.Collapsed;

        public Visibility ProgressBarVisibility
        {
            get => _ProgressBarVisibility;
            set => Set(ref _ProgressBarVisibility, value);
        }


        private string _TargetPath;

        public string TargetPath
        {
            get => _TargetPath;
            set => Set(ref _TargetPath, value);
        }

        /// <summary>
        /// Список файлов
        /// </summary>
        private List<string> _Files;

        public List<string> Files
        {
            get => _Files;
            set => Set(ref _Files, value);
        }

        public int? FilesCount => Files?.Count;

        /// <summary>
        /// Список строк результата (одна строка = один файл УП)
        /// </summary>
        private ObservableCollection<DetailRow> _DetailRows;

        public ObservableCollection<DetailRow> DetailRows
        {
            get => _DetailRows;
            set => Set(ref _DetailRows, value);
        }

        public int? DetailsCount => DetailRows?.Count;

        /// <summary>
        /// Текстовое представление результата (для сохранения в файл).
        /// </summary>
        public string DetailsText
        {
            get => BuildCsv();
        }

        /// <summary>
        /// Строковые обозначения серийных деталей, загруженные из БД
        /// </summary>
        private List<string> _SerialPartNameContains;

        /// <summary>
        /// Строка подключения к БД stanki
        /// </summary>
        private string _ConnectionString;

        public string ConnectionString
        {
            get => _ConnectionString;
            set => Set(ref _ConnectionString, value);
        }

        /// <summary>
        /// Метод поиска
        /// </summary>
        private FindMode _FindMode = FindMode.General;

        public FindMode FindMode
        {
            get => _FindMode;
            set => Set(ref _FindMode, value);
        }

        #region Команды


        #region CloseApplicationCommand
        public ICommand CloseApplicationCommand { get; }
        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }
        private bool CanCloseApplicationCommandExecute(object p) => true;
        #endregion


        #region SetPathCommand
        public ICommand SetPathCommand { get; }
        private void OnSetPathCommandExecuted(object p)
        {
            FolderBrowserDialog folderDialog = new();
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                TargetPath = folderDialog.SelectedPath;
                FindButtonEnabled = true;
            }
            else
            {
                Status = "Выбор отменен";
            }
        }
        private bool CanSetPathCommandExecute(object p) => true;
        #endregion


        #region FindDetailsCommand
        public ICommand FindDetailsCommand { get; }
        private void OnFindDetailsCommandExecuted(object p)
        {
            if (!GetFilesThreadFlag)
            {
                GetFilesThreadFlag = true;
                getFilesThread = new(() => FindPrograms(TargetPath));
                getFilesThread.Start();
            }
            else
            {
                GetFilesThreadFlag = false;
            }
        }
        private bool CanFindDetailsCommandExecute(object p) => true;
        #endregion


        #region SaveDetailsToFileCommand
        public ICommand SaveDetailsToFileCommand { get; }
        private void OnSaveDetailsToFileCommandExecuted(object p)
        {
            if (DetailsCount > 0)
            {
                SaveFileDialog saveFileDialog = new();
                saveFileDialog.Filter = "CSV-файлы (*.csv)|*.csv|Все файлы (*.*)|*.*";
                saveFileDialog.DefaultExt = "csv";
                if(saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog.FileName, BuildCsv(), new UTF8Encoding(true));
                    Status = $"Список записан в файл \"{saveFileDialog.FileName}\"";
                }
            }
        }
        private bool CanSaveDetailsToFileCommandExecute(object p) => true;
        #endregion

        #endregion



        public MainWindowViewModel()
        {
            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            SetPathCommand = new LambdaCommand(OnSetPathCommandExecuted, CanSetPathCommandExecute);
            FindDetailsCommand = new LambdaCommand(OnFindDetailsCommandExecuted, CanFindDetailsCommandExecute);
            SaveDetailsToFileCommand = new LambdaCommand(OnSaveDetailsToFileCommandExecuted, CanSaveDetailsToFileCommandExecute);

            if (string.IsNullOrWhiteSpace(ConnectionString))
                Status = "Укажите строку подключения к БД stanki, чтобы включить сопоставление с серийными деталями.";
        }

        private void FindPrograms(string path)
        {
            FindButtonText = "Остановить";
            Files = new();
            DetailRows = new();
            Status = "Подсчет файлов";
            BrowseButtonEnabled = false;
            SaveButtonEnabled = false;
            ModeComboboxEnabled = false;
            OnPropertyChanged(nameof(FilesCount));
            OnPropertyChanged(nameof(DetailsCount));
            OnPropertyChanged(nameof(DetailsText));
            ProgressBarVisibility = Visibility.Collapsed;

            _SerialPartNameContains = LoadSerialPartNames();

            GetFiles(path);
            ProgressMaxValue = (double)FilesCount;
            Progress = 0;
            ProgressBarVisibility = Visibility.Visible;
            foreach (var file in Files)
            {
                Status = "Проверка файлов на наличие УП";
                if (!GetFilesThreadFlag) 
                {
                    ProgressBarVisibility = Visibility.Collapsed;
                    break;
                }
                Progress++;
                try
                {
                    var (name, designation) = TryExtractDetail(file, TargetPath);
                    if (designation is null)
                        continue;

                    var row = new DetailRow
                    {
                        Name = name,
                        Designation = designation,
                        ProgramFile = Path.GetFileName(file),
                        Modified = File.GetLastWriteTime(file),
                        IsSerial = IsSerialDesignation(designation),
                    };
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        DetailRows.Add(row);
                        OnPropertyChanged(nameof(DetailsCount));
                        OnPropertyChanged(nameof(DetailsText));
                    });
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, e.ToString());
                }
            }
            Status = "Завершено";
            if(DetailsCount > 0) SaveButtonEnabled = true;
            BrowseButtonEnabled = true;
            ModeComboboxEnabled = true;
            FindButtonText = "Сформировать";
            GetFilesThreadFlag = false;
        }

        /// <summary>
        /// Определяет наименование и обозначение детали по файлу УП в зависимости от режима поиска.
        /// Возвращает designation == null, если файл не является программой в текущем режиме.
        /// </summary>
        private (string Name, string Designation) TryExtractDetail(string file, string targetPath)
        {
            switch (FindMode)
            {
                case FindMode.General:
                case FindMode.GeneralOnlyNumbers:
                    var generalLines = File.ReadLines(file).Take(2).ToArray();
                    if (generalLines.Length == 0 || generalLines[0] != "%")
                        return (string.Empty, null);
                    return GetNameAndDesignationFromPath(file, targetPath);
                case FindMode.QuaserOnlyNumbers:
                    if (Path.GetExtension(file).ToLowerInvariant() != ".h")
                        return (string.Empty, null);
                    var quaserLines = File.ReadLines(file).Take(2).ToArray();
                    if (quaserLines.Length == 0 || !quaserLines[0].Contains("BEGIN PGM"))
                        return (string.Empty, null);
                    return GetNameAndDesignationFromPath(file, targetPath);
                case FindMode.DirName:
                    if (!NCRenamer.Util.machineExtensions.Contains(Path.GetExtension(file).ToLower()))
                        return (string.Empty, null);
                    return (string.Empty,
                        GetDetailNameFromPath(file, targetPath, GetNameOptions.AsIs).TranslateFromEnNumber().Replace("_", " "));
                case FindMode.FileName:
                    return (string.Empty,
                        NCRenamer.Util.GetPartNameFromFileName(file).TranslateFromEnNumber().Replace(".FREZEROVKA", "").FindNumber());
                case FindMode.Mazak350:
                    return (string.Empty, NCRenamer.Util.GetMazatrolSmartName(file).TranslateFromEnNumber().FindNumber());
                default:
                    return (string.Empty, null);
            }
        }

        /// <summary>
        /// Определяет наименование и обозначение по структуре папок пути УП.
        /// </summary>
        private static (string Name, string Designation) GetNameAndDesignationFromPath(string file, string targetPath)
        {
            string cwd = file;
            while (cwd != targetPath)
            {
                cwd = Directory.GetParent(cwd).FullName;
                var folderName = Path.GetFileName(cwd);
                foreach (var sign in DetailsInfo.numberSigns)
                {
                    if (folderName.Contains(sign))
                    {
                        return (Directory.GetParent(cwd).Name, folderName);
                    }
                }
            }
            return (string.Empty, string.Empty);
        }

        /// <summary>
        /// Загружает имена серийных деталей из БД стanki.
        /// </summary>
        private List<string> LoadSerialPartNames()
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                Status = "Строка подключения не задана — сопоставление с серийными деталями выполняться не будет.";
                return new List<string>();
            }

            try
            {
                return PartsRepository.GetSerialPartDesignations(ConnectionString);
            }
            catch (Exception e)
            {
                Status = $"Не удалось прочитать БД: {e.Message}";
                return new List<string>();
            }
        }

        /// <summary>
        /// Проверяет, является ли обозначение серийной деталью (по вхождению в имена из БД).
        /// </summary>
        private bool IsSerialDesignation(string designation)
        {
            if (_SerialPartNameContains is null || _SerialPartNameContains.Count == 0)
                return false;
            if (string.IsNullOrWhiteSpace(designation))
                return false;
            return _SerialPartNameContains.Any(p =>
                !string.IsNullOrWhiteSpace(p) &&
                p.Contains(designation, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Формирует CSV-представление результата.
        /// </summary>
        private string BuildCsv()
        {
            if (DetailRows is null || DetailRows.Count == 0)
                return string.Empty;

            var sb = new StringBuilder();
            sb.AppendLine("Наименование;Обозначение;Имя файла УП;Дата изменения;Серийная");
            foreach (var row in DetailRows)
            {
                sb.AppendLine(string.Join(";",
                    CsvEscape(row.Name),
                    CsvEscape(row.Designation),
                    CsvEscape(row.ProgramFile),
                    row.Modified.ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture),
                    row.IsSerial ? "true" : "false"));
            }
            return sb.ToString().TrimEnd('\r', '\n');
        }

        private static string CsvEscape(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            if (value.Contains(';') || value.Contains('"') || value.Contains('\n'))
                return $"\"{value.Replace("\"", "\"\"")}\"";
            return value;
        }

        private void GetFiles(string path)
        {

            try
            {
                foreach (string folder in Directory.GetDirectories(path))
                {
                    if (!GetFilesThreadFlag) break;
                    GetFiles(folder);
                }
                foreach (string file in Directory.GetFiles(path))
                {
                    if (!GetFilesThreadFlag) break;
                    Files.Add(file);
                    OnPropertyChanged(nameof(FilesCount));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.ToString());
            }
        }

        public static string GetDetailNameFromPath(string file, string targetPath, GetNameOptions options = GetNameOptions.NameWithNumber)
        {
            string cwd = file;
            while (cwd != targetPath)
            {
                cwd = Directory.GetParent(cwd).FullName;
                if (options == GetNameOptions.AsIs) return Path.GetFileName(cwd);
                foreach (var sign in Infrastructure.DetailsInfo.numberSigns)
                {
                    if (Path.GetFileName(cwd).Contains(sign))
                    {
                        switch (options)
                        {
                            case GetNameOptions.NameWithNumber:
                                return $"{Directory.GetParent(cwd).Name} {Path.GetFileName(cwd)}";
                            case GetNameOptions.OnlyNumber:
                                return Path.GetFileName(cwd);
                        }
                    }
                }
            }
            return string.Empty;
            //return $"{Directory.GetParent(file).Parent.Name} {Directory.GetParent(file).Name}";
        }

    }
}
