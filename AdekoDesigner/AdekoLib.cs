using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DevExpress.XtraGrid;
using System.Text.RegularExpressions;
using System.Globalization;
using DevExpress.XtraGrid.Views.Grid;

namespace AdekoDesigner
{
    public partial class AdekoLib : DevExpress.XtraEditors.XtraForm
    {
        public AdekoLib()
        {
            InitializeComponent();

            mainDir = "C:\\Adeko 142\\"; 
            desingFolderName = ".\\Designs\\";
            libFolderName = "";
            formDesignDeleted = false;

            loadDataTableDefs();
        }

        private string mainDir, libFolderName, desingFolderName;
        public bool formDesignDeleted;

        private List<DefGroup> defGroupList = new List<DefGroup>();
        private List<AdekoModule> adekoModuleList = new List<AdekoModule>();
        private BindingList<AdekoModule> adekoModuleList_canRead = new BindingList<AdekoModule>();
        private BindingList<AdekoModule> adekoModuleList_cantRead = new BindingList<AdekoModule>();

        DataTable dataDefs = null;


        private void gridControl2_Load(object sender, EventArgs e)
        {
            //loadRowData(1);
        }

        private void loadDataTableDefs()
        {
            dataDefs = new DataTable();
            dataDefs.Columns.Add("Code", typeof(string)); // Modül Kodu
            dataDefs.Columns.Add("Group", typeof(string)); // Grup
            dataDefs.Columns.Add("Width", typeof(decimal)); // Genişlik
            dataDefs.Columns.Add("Height", typeof(decimal)); // Yükseklik
            dataDefs.Columns.Add("Depth", typeof(decimal)); // Derinlik
            dataDefs.Columns.Add("Description", typeof(string)); // Açıklama
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveDefFiles();
        }

        private void SaveDefFiles()
        {
            foreach (DefGroup defGroup in defGroupList)
            {
                try
                {
                    string defFilePath = Path.Combine(mainDir, libFolderName, $"{defGroup.code}.DEF");

                    if (!File.Exists(defFilePath))
                    {
                        MessageBox.Show($"'{defGroup.code}.DEF' dosyası bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    List<AdekoModule> adekoModuleList_ToSave = combineLists(adekoModuleList_canRead, adekoModuleList_cantRead);

                    // Bu .DEF dosyasına ait güncellenmiş satırları al
                    List<AdekoModule> related_adekoModuleDefList = adekoModuleList_ToSave
                        .Where(row => row.FileName == defGroup.code)
                        .OrderBy(row => row.FileRowNo)
                        .ToList();

                    //Liste boş ise devam etme
                    if (related_adekoModuleDefList.Count == 0) continue;
                    // Güncellenmiş satırları al
                    var updatedRows = GetUpdatedRows(related_adekoModuleDefList);
                    if (updatedRows.Count == 0) continue;

                    try
                    {
                        // Mevcut dosyayı oku
                        //var originalLines = File.ReadAllLines(defFilePath);

                        // Yeni dosya içeriğini oluştur
                        var newLines = CreateUpdatedFileContent(defGroup, related_adekoModuleDefList);

                        // Yeni içeriği dosyaya yaz
                        using (var writer = new StreamWriter(defFilePath, false, Encoding.Default))
                        {
                            for (int i = 0; i < newLines.Count; i++)
                            {
                                if (i == newLines.Count - 1) // Last line
                                {
                                    writer.Write(newLines[i]); // Write without adding a new line
                                }
                                else
                                {
                                    writer.WriteLine(newLines[i]);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"'{defGroup.code}.DEF' dosyasını güncellerken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } catch (Exception) { }
                
            }

            MessageBox.Show("Dosyalar başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private List<AdekoModule> combineLists(BindingList<AdekoModule> list1, BindingList<AdekoModule> list2)
        {
            List<AdekoModule> combinedList = new List<AdekoModule>();

            foreach (var module in list1)
            {
                combinedList.Add(module);
            }

            foreach (var module in list2)
            {
                combinedList.Add(module);
            }
            return combinedList;
        }

        // Dosya içeriğini güncelleyen yardımcı metod
        private List<string> CreateUpdatedFileContent(DefGroup defGroup, List<AdekoModule> related_adekoModuleDefList)
        {
            var newLines = new List<string>();

            foreach (var line in related_adekoModuleDefList)
            {
                newLines.Add(FormatRowData(line));
            }

            return newLines;
        }

        private string FormatRowData(AdekoModule row)
        {
            // Örnek: Satırdaki verileri CSV formatında yaz
            //return $@"(""{row.Code}"" ""{row.Dt1}"" {row.Width} {row.Height} {row.Depth} {row.DynamicDataString} ""{row.RootCode}"" ""{row.Description}"")";
            if (string.IsNullOrEmpty(row.OriginalDataLine))
            {
                return "";
            }
            else if (row.IsUpdated)
            {
                if (row.moduleType == ModuleType.std)
                {
                    return $@"(""{row.Code}"" {row.Dt1} {row.Width?.ToString("F4") ?? "0.0000"} {row.Height?.ToString("F4") ?? "0.0000"} {row.Depth?.ToString("F4") ?? "0.0000"} {row.DynamicDataString} ""{row.RootCode}"" ""{row.Description}"")";
                }
                else if (row.moduleType == ModuleType.korKose)
                {
                    return $@"(""{row.Code}"" {row.Dt1} {row.DynamicDataString} ""{row.RootCode}"" ""{row.Description}"")";
                }
                else if (row.moduleType == ModuleType.diger)
                {
                    return $@"(""{row.Code}"" {row.Dt1} {row.DynamicDataString} ""{row.Description}"")";
                }
                else
                {
                    return "(" + row.OriginalDataLine + ")";
                }
            }
            else
            {
                return "(" + row.OriginalDataLine + ")";
            }


        }



        private List<AdekoModule> GetUpdatedRows(List<AdekoModule> mList)
        {
            var updatedRows = new List<AdekoModule>();
            for (int i = 0; i < mList.Count; i++)
            {
                if (mList[i].IsUpdated)
                {
                    updatedRows.Add(mList[i]);
                }
            }
            return updatedRows;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }



        public void RefreshData()
        {
            List<string> ignoredFolders = new List<string> { "kapak", "kulp", "cera", "adeko_render_viewer32", "adeko_render_viewer64", "dclImages", "iconengines", "imageformats", "lights", "materials", "platforms", "ADEData", "adeko_render_viewer", "Agrx", "btoolsets", "Fonts", "Help", "Imalat", "lang", "language", "lng", "logs", "Patterns", "Shaders", "tefris", "xmf_tr", ".git" };

            try
            {
                // Ana dizini kontrol et
                if (!Directory.Exists(mainDir)) { throw new DirectoryNotFoundException($"'{mainDir}' dizini bulunamadı."); }

                // Tüm klasörleri al
                string[] allDirectories = Directory.GetDirectories(mainDir);

                // DataTable oluştur
                DataTable dtExcludedFolders = new DataTable();
                dtExcludedFolders.Columns.Add("LIB_NAME", typeof(string));

                foreach (string dir in allDirectories)
                {
                    string folderName = Path.GetFileName(dir);

                    // Eğer klasör ignoredFolders içinde değilse, DataTable'a ekle
                    if (!ignoredFolders.Contains(folderName))
                    {
                        DataRow row = dtExcludedFolders.NewRow();
                        row["LIB_NAME"] = folderName;
                        dtExcludedFolders.Rows.Add(row);
                    }
                }

                // DataTable'ı gridControl1'e ata
                gridControl1.DataSource = dtExcludedFolders;
            }
            catch (FileNotFoundException ex) { MessageBox.Show(ex.Message, "Dosya Bulunamadı", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            catch (Exception ex) { MessageBox.Show($"Beklenmeyen bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            loadRowData(e.RowHandle);
        }

        private void loadRowData(int rowHandle)
        {
            // Seçilen satırdan kodu ve yolu al
            DataRow selectedRow = gridView1.GetDataRow(rowHandle);
            if (selectedRow != null)
            {
                libFolderName = selectedRow["LIB_NAME"].ToString();

                defGroupList.Clear();
                adekoModuleList.Clear();

                LoadGroupLst(libFolderName);

                splashScreenManager1.ShowWaitForm();

                if (defGroupList.Count > 0)
                {
                    foreach (var defGroup in defGroupList)
                    {
                        addDefFile_toDataTableDefs(defGroup);
                    }

                    adekoModuleList_canRead = new BindingList<AdekoModule>(adekoModuleList.Where(module => module.canRead).ToList());
                    adekoModuleList_cantRead = new BindingList<AdekoModule>(adekoModuleList.Where(module => !module.canRead).ToList());

                    gridControl2.DataSource = adekoModuleList_canRead;
                    gridControl2.RefreshDataSource();
                }

                splashScreenManager1.CloseWaitForm();
            }
        }

        private void LoadGroupLst(string libFolder)
        {
            try
            {
                // GRUP.LST dosyasının tam yolu
                string groupLstPath = Path.Combine(mainDir, libFolder, "GRUPS.LST");

                if (!File.Exists(groupLstPath)) { throw new FileNotFoundException($"'{groupLstPath}' dosyası bulunamadı."); }

                using (StreamReader reader = new StreamReader(groupLstPath, Encoding.GetEncoding("windows-1254")))
                {
                    string[] lines = reader.ReadToEnd().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                    foreach (string line in lines)
                    {
                        Match match = Regex.Match(line, "\\(\"(.*?)\"\\s+\"(.*?)\"\\)");
                        if (match.Success)
                        {
                            DefGroup defGroup = new DefGroup()
                            {
                                code = match.Groups[1].Value,
                                name = match.Groups[2].Value
                            };

                            defGroupList.Add(defGroup);
                        }
                    }
                }

            }
            catch (FileNotFoundException ex) { MessageBox.Show(ex.Message, "Dosya Bulunamadı", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            catch (Exception ex) { MessageBox.Show($"Beklenmeyen bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        public void addDefFile_toDataTableDefs(DefGroup defGroup)
        {

            string defFilePath = Path.Combine(mainDir, libFolderName, $"{defGroup.code}.DEF");
            if (!File.Exists(defFilePath))
            {
                MessageBox.Show($"'{defGroup.code}.DEF' dosyası bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int rowNo;
            try
            {
                string[] lines = File.ReadAllLines(defFilePath, Encoding.GetEncoding("windows-1254")); // Tüm dosyayı okuyun
                rowNo = 0;

                foreach (var line in lines)
                {
                    rowNo++;
                    AdekoModule adekoModule = ParseLine(line, defGroup, rowNo);

                    if (adekoModule != null)
                    {
                        adekoModuleList.Add(adekoModule);
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata loglama veya işlem
            }
        }

        private AdekoModule ParseLine(string line, DefGroup defGroup, int rowNo)
        {
            try
            {
                // Satırı düzenle
                line = line.Trim().Trim('(', ')');
                List<string> parts = SplitWithQuotes(line);

                if (parts.Count < 4 || parts[0].Trim('"') == ".") // Geçersiz satır
                    return CreateInvalidModule(defGroup, rowNo, line);

                // Verileri ayıkla
                string code = removeQuetes(parts[0].Trim('"'));
                string dt1 = removeQuetes(parts[1].Trim('"'));
                string keyCode = removeQuetes(parts[parts.Count - 2].Trim('"'));
                string description = removeQuetes(parts[parts.Count - 1].Trim('"'));

                ModuleType _moduleType;

                // Genişlik, yükseklik ve derinlik
                decimal? width, height, depth ;
                string dynamicDataString = "";

                if (parts[2] == "(QUOTE") // Diğerleri
                {
                    _moduleType = ModuleType.diger;
                    keyCode = "";

                    // Dinamik verileri ayıkla
                    int startIndex = 2;
                    int count = Math.Max(0, parts.Count - startIndex - 1);
                    dynamicDataString = string.Join(" ", parts.Skip(startIndex).Take(count));

                    // Genişlik, yükseklik ve derinlik
                    width = null;
                    height = null;
                    depth = null;
                }
                else if (parts[3] == "nil") // Kör Köşe
                {
                    _moduleType = ModuleType.korKose;

                    // Dinamik verileri ayıkla
                    int startIndex = 2;
                    int count = Math.Max(0, parts.Count - startIndex - 2);
                    dynamicDataString = string.Join(" ", parts.Skip(startIndex).Take(count));

                    // Genişlik, yükseklik ve derinlik
                    width = null;
                    height = null;
                    depth = null;
                    //width = TryParseDecimal(parts.ElementAtOrDefault(7));
                    //height = TryParseDecimal(parts.ElementAtOrDefault(8));
                    //depth = TryParseDecimal(parts.ElementAtOrDefault(9));
                }
                else // std modül
                {
                    _moduleType = ModuleType.std;

                    // Dinamik verileri ayıkla
                    int startIndex = 5;
                    int count = Math.Max(0, parts.Count - startIndex - 2);
                    dynamicDataString = string.Join(" ", parts.Skip(startIndex).Take(count));

                    // Genişlik, yükseklik ve derinlik
                    width = TryParseDecimal(parts.ElementAtOrDefault(2));
                    height = TryParseDecimal(parts.ElementAtOrDefault(3));
                    depth = TryParseDecimal(parts.ElementAtOrDefault(4));
                }

                

                // Modülü oluştur
                return new AdekoModule
                {
                    canRead = true,
                    SelectModule = "Modül Seç ->",
                    Code = code,
                    Group = defGroup.name,
                    Dt1 = dt1,
                    Width = width,
                    Height = height,
                    Depth = depth,
                    RootCode = keyCode,
                    Description = description,
                    LibFolderName = libFolderName,
                    FileName = defGroup.code,
                    FileRowNo = rowNo,
                    IsUpdated = false,
                    OriginalDataLine = line,
                    DynamicDataString = dynamicDataString,
                    moduleType = _moduleType
                };
                //return null;
            }
            catch
            {
                return CreateInvalidModule(defGroup, rowNo, line);
            }
        }

        private AdekoModule CreateInvalidModule(DefGroup defGroup, int rowNo, string line)
        {
            return new AdekoModule
            {
                canRead = false,
                Group = defGroup.name,
                FileName = defGroup.code,
                FileRowNo = rowNo,
                OriginalDataLine = line,
                moduleType = ModuleType.tanimsiz
            };
        }

        // Decimal parse etmek için yardımcı fonksiyon
        private decimal? TryParseDecimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            value = removeQuetes(value);

            // Replace comma with dot for consistency in decimal parsing
            value = value.Replace(',', '.');

            return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result)
                   ? (decimal?)result
                   : null;
        }


        private static List<string> SplitWithQuotes(string input)
        {
            // Regex: Çift tırnak içindeki ifadeleri veya tırnaksız ifadeleri yakalar
            var matches = Regex.Matches(input, @"\"".*?\""|[^\s]+");
            var result = new List<string>();

            foreach (Match match in matches)
            {
                string value = match.Value;

                result.Add(value);
            }

            return result;
        }

        private string removeQuetes(string value)
        {
            // Eğer çift tırnak içindeyse, tırnakları kaldır
            if (value.StartsWith("\"") && value.EndsWith("\""))
            {
                value = value.Substring(1, value.Length - 2);
            }

            return value;
        }


        private void gridView2_RowClick(object sender, RowClickEventArgs e)
        {
            int rowHandle = gridView2.FocusedRowHandle;
            var rowObject = gridView2.GetRow(rowHandle);
            if (rowObject != null)
            {
                var myObject = rowObject as AdekoModule; //  DataSource'ta kullanılan tür
                if (myObject != null)
                {
                    string code = myObject.Code;
                    string folderName = myObject.FileName + "_tn";
                    string libFolderName = myObject.LibFolderName;

                    if (libFolderName != null && code != null)
                    {

                        string imgFilePath = Path.Combine(mainDir, libFolderName, folderName, $"{code}.bmp");

                        // Dosyanın mevcut olup olmadığını kontrol et
                        if (File.Exists(imgFilePath))
                        {
                            // Resmi PictureEdit kontrolünde göster
                            pictureEdit1.Image = Image.FromFile(imgFilePath);
                        }
                        else
                        {
                            //MessageBox.Show($"Görsel bulunamadı: {imgFilePath}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            pictureEdit1.Image = Properties.Resources.resim_yok; // Resim bulunamadığında kontrolü temizle
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Seçilen satır uygun türde değil.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Lütfen geçerli bir veri satırı seçin.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool isUpdatingCellValue = false; // Sonsuz döngüyü önlemek için flag
        private void gridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (isUpdatingCellValue) return; // Eğer flag true ise işlem yapmadan çık

            try
            {
                isUpdatingCellValue = true; // İşlem başladığında flag'i true yap
                // Değişen satırın "IsUpdated" sütununu true olarak işaretle
                gridView2.SetRowCellValue(e.RowHandle, "IsUpdated", true);
            }
            finally
            {
                isUpdatingCellValue = false; // İşlem tamamlandığında flag'i false yap
            }

        }

        private void AdekoLib_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveFormDesigns();
        }
        private void AdekoLib_Load(object sender, EventArgs e)
        {
            LoadFormDesigns();
        }

        private void btnResetDesigns_Click(object sender, EventArgs e)
        {
            ReseTFormDesigns();
        }


        private void SaveFormDesigns()
        {
            if (!formDesignDeleted)
            {
                try { splitContainerControl1.SaveLayoutToXml(desingFolderName + "splitContainerControl1.xml"); } catch (Exception) { }
                try { splitContainerControl2.SaveLayoutToXml(desingFolderName + "splitContainerControl2.xml"); } catch (Exception) { }
                try { gridView1.SaveLayoutToXml(desingFolderName + "gridView1.xml"); } catch (Exception) { }
                try { gridView2.SaveLayoutToXml(desingFolderName + "gridView2.xml"); } catch (Exception) { }
            }
        }

        private void LoadFormDesigns()
        {
            try { splitContainerControl1.RestoreLayoutFromXml(desingFolderName + "splitContainerControl1.xml"); } catch (Exception) { }
            try { splitContainerControl2.RestoreLayoutFromXml(desingFolderName + "splitContainerControl2.xml"); } catch (Exception) { }
            try { gridView1.RestoreLayoutFromXml(desingFolderName + "gridView1.xml"); } catch (Exception) { }
            try { gridView2.RestoreLayoutFromXml(desingFolderName + "gridView2.xml"); } catch (Exception) { }
        }


        private void ReseTFormDesigns()
        {

            DialogResult dialog = MessageBox.Show(this, "Form kapatılacaktır. Sonra tekrar açmalısınız. Onaylıyor musunuz?", "ÇIKIŞ", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                formDesignDeleted = true;
                if (System.IO.File.Exists(desingFolderName + "splitContainerControl1.xml")) { System.IO.File.Delete(desingFolderName + "splitContainerControl1.xml"); }
                if (System.IO.File.Exists(desingFolderName + "splitContainerControl2.xml")) { System.IO.File.Delete(desingFolderName + "splitContainerControl2.xml"); }
                if (System.IO.File.Exists(desingFolderName + "gridView1.xml")) { System.IO.File.Delete(desingFolderName + "gridView1.xml"); }
                if (System.IO.File.Exists(desingFolderName + "gridView2.xml")) { System.IO.File.Delete(desingFolderName + "gridView2.xml"); }

                this.Close();
            }
            else
            {
                MessageBox.Show(this, "Çıkış yapılmadı");
            }
        }


        private void btnExcelExport_Click(object sender, EventArgs e)
        {
            ExportGridToExcel();
        }


        private void ExportGridToExcel()
        {
            string lastUsedFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Varsayılan başlangıç

            // SaveFileDialog oluştur
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                // Dialog ayarlarını yapılandır
                saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveFileDialog.Title = "Excel Dosyasını Kaydet";

                string sanitizedDateTime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                saveFileDialog.FileName =$@"{libFolderName} Mutfak Modülleri - {sanitizedDateTime}.xlsx";

                saveFileDialog.InitialDirectory = lastUsedFolderPath;

                // Kullanıcı bir dosya seçtiyse
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;

                    try
                    {
                        // Grid'i Excel'e dışa aktar
                        gridControl2.ExportToXlsx(filePath);

                        // Kullanıcıya işlem tamamlandığını bildir
                        //MessageBox.Show($"Grid başarıyla Excel dosyasına dışa aktarıldı.\nDosya Yolu: {filePath}", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Dosyayı aç (isteğe bağlı)
                        //System.Diagnostics.Process.Start(filePath);
                    }
                    catch (Exception ex)
                    {
                        // Hata durumunda mesaj göster
                        MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


    }

    public class DefGroup
    {
        public string code {get; set;}
        public string name {get; set;}
    }
    public class AdekoModule
    {
        public bool canRead { get; set; }
        public string SelectModule { get; set; }
        public string Code { get; set; }
        public string Dt1 { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal? Depth { get; set; }
        public string RootCode { get; set; }
        public string Description { get; set; }
        //public List<string> DynamicData { get; set; } = new List<string>();
        public string DynamicDataString { get; set; } // Dinamik verilerin birleşik hali
        public string LibFolderName { get; set; }
        public string Group { get; set; }
        public string FileName { get; set; }
        public int FileRowNo { get; set; }
        public bool IsUpdated { get; set; }
        public string OriginalDataLine { get; set; } // Orijinal veri (değişmeyen kısımlar)
        public ModuleType moduleType { get; set; }
    }

    public enum ModuleType
    {
        tanimsiz = 0,
        std = 1,
        korKose = 2,
        diger = 3
    }

}