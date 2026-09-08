using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace Cutter
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string _fileName;
        private long _fileSize;
        private string _lastXy;
        private int _lastXyIndex;
        private int _parts;
        private int _headEndPoint;
        private int _endingStartPoint;
        private int _partStrings;
        private int _startLine;
        private int _cutLine;
        public List<string> Body = new List<string>();
        public List<string> Head = new List<string>();
        public List<string> Ending = new List<string>();
        private string[] _lines;
        private string _feed;

        private void OpenFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            Body.Clear();
            Head.Clear();
            Ending.Clear();
            _feed = "";
            label7.Visible = false;
            textBox5.Visible = false;
            button5.Visible = false;
            _headEndPoint = 0;
            _endingStartPoint = 0;
            _fileName = openFileDialog1.FileName;
            textBox2.Text = openFileDialog1.SafeFileName;
            _fileSize = new FileInfo(_fileName).Length;
            textBox3.Text = "";
            _lines = File.ReadAllLines(_fileName);
            numericUpDown1.Maximum = _lines.Length;
            numericUpDown2.Maximum = _lines.Length;
            _headEndPoint = 0;
            foreach (string line in _lines)
            {
                if (line.Contains("F") && line.Contains("G1"))
                {
                    break;
                }

                Head.Add(line);
                _headEndPoint++;
            }
            numericUpDown1.Value = Convert.ToDecimal(_headEndPoint);
            foreach (string line in _lines)
            {
                if (line.Contains("F") && line.Contains("G1"))
                {
                    _feed = line.Remove(0, line.IndexOf('F'));
                    break;
                }
            }
            if (_feed == null || _feed == "")
            {
                label7.Visible = true;
                textBox5.Visible = true;
                button5.Visible = true;
            }

            _endingStartPoint = 0;
            Array.Reverse(_lines);
            foreach (string line in _lines)
            {
                if (line.Contains("G0"))
                {
                    _endingStartPoint++;
                    break;
                }

                _endingStartPoint++;
            }
            Array.Reverse(_lines);
            _endingStartPoint = _lines.Length - _endingStartPoint;
            numericUpDown2.Value = Convert.ToDecimal(_endingStartPoint) + 1;
            string[] endLines = new string[_lines.Length - _endingStartPoint];
            Array.Copy(_lines, _endingStartPoint, endLines, 0, (_lines.Length - _endingStartPoint));

            RefreshInfoText();

        }

        private int GetMaxSizeKb()
        {
            if (numericUpDownMaxSize == null)
                return 480;
            int value = Convert.ToInt32(numericUpDownMaxSize.Value);
            return value > 0 ? value : 480;
        }

        private void NumericUpDownMaxSize_ValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_fileName) || _lines == null || _lines.Length == 0)
                return;

            RefreshInfoText();
        }

        private void RefreshInfoText()
        {
            int maxSizeKb = GetMaxSizeKb();
            long fileSizeKb = _fileSize / 1024;
            _parts = Convert.ToInt32(fileSizeKb) / maxSizeKb + 1;
            _partStrings = _lines.Length / _parts;

            textBox1.Text = "";
            textBox1.Text += $"Открыт файл:...........{_fileName}\r\n";
            textBox1.Text += $"Общий размер:..........{fileSizeKb} Кб\r\n";
            textBox1.Text += $"Допустимый размер:.....{maxSizeKb} Кб\r\n";
            textBox1.Text += $"Частей:................{_parts}\r\n";
            textBox1.Text += $"Строк всего:...........{_lines.Length}\r\n";
            textBox1.Text += $"Строк в каждой части:..≈ {FormatPartStrings()}k\r\n";
            long partKb = EstimatePartSizeKb();
            textBox1.Text += $"Расчётный вес части:...≈ {partKb} Кб{(partKb > maxSizeKb ? " (превышает лимит!)" : "")}\r\n";
            if (!string.IsNullOrEmpty(_feed))
            {
                textBox1.Text += $"Подача:................{_feed}\r\n\r\n";
            }
            else
            {
                textBox1.AppendText("Подача не обнаружена. Можно установить подачу в появившейся форме." + Environment.NewLine);
            }

            bool needSplit = fileSizeKb >= maxSizeKb;
            button4.Enabled = needSplit;
            numericUpDown1.Enabled = needSplit;
            numericUpDown2.Enabled = needSplit;
            button2.Enabled = needSplit;
            button3.Enabled = needSplit;
            textBox3.Enabled = needSplit;
            textBox4.Enabled = needSplit;
            if (!needSplit)
            {
                textBox1.Text = "Файл нормального размера, ничего разделять не надо.";
                textBox3.Text = "";
                textBox4.Text = "";
            }
        }

        private string FormatPartStrings()
        {
            string s = _partStrings.ToString();
            if (s.Length <= 3)
                return s;
            return s.Remove(s.Length - 3, 3);
        }

        private long EstimatePartSizeKb()
        {
            if (_lines == null || _lines.Length == 0 || _parts <= 0)
                return 0;
            long headBytes = GetLinesByteSize(Head);
            long endingBytes = GetLinesByteSize(Ending);
            int bodyLines = _lines.Length - Head.Count - Ending.Count;
            if (bodyLines < 0)
                bodyLines = 0;
            double avgBytesPerLine = (double)_fileSize / _lines.Length;
            double partBytes = headBytes + endingBytes + avgBytesPerLine * bodyLines / _parts;
            return (long)System.Math.Ceiling(partBytes / 1024);
        }

        private static long GetLinesByteSize(List<string> lines)
        {
            if (lines == null || lines.Count == 0)
                return 0;
            long size = 0;
            foreach (string line in lines)
                size += System.Text.Encoding.UTF8.GetByteCount(line ?? "") + 2; // + CRLF
            return size;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            PrintHead();
            PrintEnding();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Head.Clear();
            Head.AddRange(textBox3.Text.Split((string[])null, StringSplitOptions.RemoveEmptyEntries));
            textBox3.Clear();
            Head.RemoveAt(Head.Count - 1);
            PrintHead();
            textBox1.AppendText("Измененная шапка сохранена." + Environment.NewLine);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Ending.Clear();
            Ending.AddRange(textBox4.Text.Split((string[])null, StringSplitOptions.RemoveEmptyEntries));
            textBox4.Clear();
            Ending.RemoveAt(Ending.Count - 1);
            PrintEnding();
            textBox1.AppendText("Измененная концовка сохранена." + Environment.NewLine);
        }


        private void PrintHead()
        {
            textBox3.Text = "";
            foreach (string line in Head)
            {
                textBox3.AppendText(line + Environment.NewLine);
            }
        }
        private void PrintEnding()
        {
            textBox4.Text = "";
            foreach (string line in Ending)
            {
                textBox4.AppendText(line + Environment.NewLine);
            }
        }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int count = 0;
            _headEndPoint = Convert.ToInt32(numericUpDown1.Value);
            Head.Clear();
            foreach (string line in _lines)
            {
                if (count < _headEndPoint)
                {
                    Head.Add(line);
                    count++;
                }
                else
                {
                    break;
                }
            }
            PrintHead();
        }

        private void NumericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            _endingStartPoint = Convert.ToInt32(numericUpDown2.Value) - 1;
            Ending.Clear();
            string[] endLines = new string[_lines.Length - _endingStartPoint];
            Array.Copy(_lines, _endingStartPoint, endLines, 0, (_lines.Length - _endingStartPoint));
            Ending.AddRange(endLines);
            PrintEnding();
        }

        private void SaveBody()
        {
            string[] tempBody = new string[_lines.Length - Head.Count - Ending.Count];
            Array.Copy(_lines, _headEndPoint, tempBody, 0, _lines.Length - _headEndPoint - (_lines.Length - _endingStartPoint));
            Body.AddRange(tempBody);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            SaveBody();
            _cutLine = 0;
            textBox1.AppendText($"Название детали определено: {FindName(Head)}" + Environment.NewLine);
            textBox1.AppendText($"Тело программы определено строками {numericUpDown1.Value} - {numericUpDown2.Value}. Всего {Body.Count} строк." + Environment.NewLine);
            for (int i = 1; i < _parts + 1; i++)
            {
                CutPart(i);
            }
        }

        private void CutPart(int part)
        {
            using (var stream = new StreamWriter($"{openFileDialog1.FileName}-{part}", false))
            {
                _startLine = _cutLine;
                if (part != _parts)
                {
                    _cutLine += _partStrings;

                    textBox1.AppendText($"\r\nСоставление части: {part}" + Environment.NewLine);
                    textBox1.AppendText($"Создание файла:................{openFileDialog1.FileName}-{part}" + Environment.NewLine);
                    textBox1.AppendText($"Изменение названия на:.........{FindName(Head)}-{part}" + Environment.NewLine);

                    //
                    // запись шапки
                    //
                    string newName = FindName(Head);
                    foreach (string line in Head)
                    {
                        // замена имени
                        if (line.Contains("(") || line.Contains(")"))
                        {
                            if (part == 1)
                            {
                                stream.WriteLine(line.Substring(0, line.Length - 1) + $"-{part})");
                            }
                            else if (part >= 10)
                            {
                                stream.WriteLine(line.Substring(0, line.Length - 4) + $"-{part})");
                            }
                            else
                            {
                                stream.WriteLine(line.Substring(0, line.Length - 3) + $"-{part})");
                            }
                        }
                        // замена XY
                        else if (line.Contains("X") && line.Contains("Y") && part > 1)
                        {
                            if (!_lastXy.Contains("G0"))
                            {
                                _lastXy = $"{_lastXy.Substring(0, _lastXy.IndexOf("X"))}G0{_lastXy.Substring(_lastXy.IndexOf("X"), _lastXy.Length - _lastXy.IndexOf("X"))}";
                            }
                            stream.WriteLine(_lastXy.Replace("G1", "G0").Replace("G2", "G0").Replace("G3", "G0"));
                            textBox1.AppendText($"Заменена координат X, Y :......{_lastXy.Replace("G1", "G0").Replace("G2", "G0").Replace("G3", "G0")}" + Environment.NewLine);
                        }
                        else
                        {
                            stream.WriteLine(line);
                        }
                    }

                    textBox1.AppendText($"Смещение для поиска на:........[{_cutLine}] {Body[_cutLine]}" + Environment.NewLine);
                    int count = 0;
                    foreach (string line in Body)
                    {

                        if (count >= _cutLine && line.Contains("X") && line.Contains("Y"))
                        {
                            _lastXyIndex = count;
                            _lastXy = line;
                        }
                        else if (count >= _cutLine && line.Contains("G0"))
                        {
                            _cutLine = count;
                            break;
                        }
                        count++;
                    }
                    textBox1.AppendText($"Последние координаты X, Y:.....[{_lastXyIndex}] {_lastXy}" + Environment.NewLine);
                    textBox1.AppendText($"Место разделения:..............[{_cutLine}] {Body[_cutLine]}" + Environment.NewLine);

                    //
                    // запись тела
                    //
                    string[] tempBody = Body.GetRange(_startLine, _cutLine - _startLine).ToArray();
                    bool feedIsWrited = false;
                    foreach (string line in tempBody)
                    {
                        // добавление подачи
                        if (!feedIsWrited && line.Contains("G1") && part != 1)
                        {
                            stream.WriteLine(line + _feed);
                            feedIsWrited = true;
                            textBox1.AppendText($"Добавлена подача:..............{line + _feed}" + Environment.NewLine);
                        }
                        else
                        {
                            stream.WriteLine(line);
                        }
                    }

                    //
                    // запись концовки
                    //
                    foreach (string line in Ending)
                    {
                        if (line != "\r\n")
                        {
                            stream.WriteLine(line);
                        }
                    }
                    textBox1.AppendText("Часть составлена." + Environment.NewLine);
                }
                else
                {
                    textBox1.AppendText($"\r\nСоставление части: {part}" + Environment.NewLine);
                    textBox1.AppendText("Смещение для поиска не нужно, последняя часть." + Environment.NewLine);
                    textBox1.AppendText($"Создание файла:................{openFileDialog1.FileName}-{part}" + Environment.NewLine);
                    textBox1.AppendText($"Изменение названия на:.........{FindName(Head)}-{part}" + Environment.NewLine);
                    string newName = FindName(Head);
                    //
                    // запись шапки
                    //
                    foreach (string line in Head)
                    {
                        // замена имени
                        if (line.Contains("(") || line.Contains(")"))
                        {
                            if (part == 1)
                            {
                                stream.WriteLine(line.Substring(0, line.Length - 1) + $"-{part})");
                            }
                            else if (part >= 10)
                            {
                                stream.WriteLine(line.Substring(0, line.Length - 4) + $"-{part})");
                            }
                            else
                            {
                                stream.WriteLine(line.Substring(0, line.Length - 3) + $"-{part})");
                            }
                        }
                        // замена XY
                        else if (line.Contains("X") && line.Contains("Y") && part > 1)
                        {
                            if (!_lastXy.Contains("G0"))
                            {
                                _lastXy = $"{_lastXy.Substring(0, _lastXy.IndexOf("X"))}G0{_lastXy.Substring(_lastXy.IndexOf("X"), _lastXy.Length - _lastXy.IndexOf("X"))}";
                            }
                            stream.WriteLine(_lastXy.Replace("G1", "G0").Replace("G2", "G0").Replace("G3", "G0"));
                            textBox1.AppendText($"Заменена координат X, Y:.......{_lastXy.Replace("G1", "G0").Replace("G2", "G0").Replace("G3", "G0")}" + Environment.NewLine);
                        }
                        else
                        {
                            stream.WriteLine(line);
                        }
                    }

                    //
                    // запись тела
                    //
                    string[] tempBody = Body.GetRange(_startLine, Body.Count - _startLine).ToArray();
                    bool feedIsWrited = false;
                    foreach (string line in tempBody)
                    {
                        // добавление подачи
                        if (!feedIsWrited && line.Contains("G1"))
                        {
                            stream.WriteLine(line + _feed);
                            feedIsWrited = true;
                            textBox1.AppendText($"Добавлена подача:..............{line + _feed}" + Environment.NewLine);
                        }
                        else
                        {
                            stream.WriteLine(line);
                        }
                    }

                    //
                    // запись концовки
                    //
                    foreach (string line in Ending)
                    {
                        stream.WriteLine(line);
                    }
                    textBox1.AppendText("Программа разделена." + Environment.NewLine);
                }
            }
        }

        private string FindName(List<string> head)
        {
            string name = "PART NAME";
            foreach (string line in head)
            {
                if (line.Contains("(") || line.Contains(")"))
                {
                    name = line.Substring(line.IndexOf("(") + 1, line.Length - line.IndexOf("(") - 2);
                    break;
                }
            }

            return name;
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            _feed = textBox5.Text.Contains("F") ? textBox5.Text : $"F{textBox5.Text}";
            textBox1.AppendText($"Установлена подача:....{_feed}" + Environment.NewLine);
        }
    }
}
