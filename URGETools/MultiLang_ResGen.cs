using DevExpress.XtraEditors;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace URGETools
{
    public partial class MultiLang_ResGen : DevExpress.XtraEditors.XtraForm
    {
        public MultiLang_ResGen()
        {
            InitializeComponent();
        }

        private void Log(string message)
        {
            txtOutput.AppendText($"{DateTime.Now:HH:mm:ss} - {message}{Environment.NewLine}");
        }
        private Form CreateFormInstance(string formClassName, string formFilePath)
        {
            try
            {
                // 1. Kaynak kod dosyasını oku
                string formCode = File.ReadAllText(formFilePath);
                if (string.IsNullOrWhiteSpace(formCode))
                {
                    Log($"Form dosyası boş veya geçersiz: {formFilePath}");
                    return null;
                }

                // 2. Form sınıfını bul (class ile başlayan kısmı arıyoruz)
                var className = ExtractClassName(formCode);
                if (!className.Equals(formClassName, StringComparison.OrdinalIgnoreCase))
                {
                    Log($"Sınıf adı eşleşmedi: {formFilePath}");
                    return null;
                }

                // 3. CSharpCodeProvider ile sınıfı derle
                var provider = new CSharpCodeProvider();
                var parameters = new CompilerParameters { GenerateInMemory = true, TreatWarningsAsErrors = false };
                var compileResults = provider.CompileAssemblyFromSource(parameters, formCode);

                if (compileResults.Errors.HasErrors)
                {
                    var errors = string.Join(Environment.NewLine, compileResults.Errors.Cast<CompilerError>().Select(e => e.ErrorText));
                    Log($"Derleme hataları: {errors}");
                    return null;
                }

                // 4. Derlenen assembly'den sınıfı al
                var assembly = compileResults.CompiledAssembly;
                var type = assembly.GetTypes().FirstOrDefault(t => t.Name.Equals(formClassName, StringComparison.OrdinalIgnoreCase) && typeof(Form).IsAssignableFrom(t));

                if (type != null)
                {
                    // 5. Formu oluştur
                    var form = (Form)Activator.CreateInstance(type);
                    Log($"Form oluşturuldu: {formClassName}");
                    return form;
                }

                Log($"Sınıf bulunamadı: {formClassName}");
                return null;
            }
            catch (Exception ex)
            {
                Log($"Hata (CreateFormInstance): {ex.Message}");
                return null;
            }
        }

        private string ExtractClassName(string formCode)
        {
            // Kodun başında veya ortasında class adı yer alır, bunu yakalıyoruz
            var classKeyword = "class ";
            var startIndex = formCode.IndexOf(classKeyword, StringComparison.OrdinalIgnoreCase);
            if (startIndex == -1) return string.Empty;

            var classNameStart = startIndex + classKeyword.Length;
            var classNameEnd = formCode.IndexOfAny(new char[] { ' ', '{' }, classNameStart);
            return formCode.Substring(classNameStart, classNameEnd - classNameStart).Trim();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            string formsFolder = textBoxFormsFolder.Text;
            string targetFolder = textBoxTargetFolder.Text;

            if (!Directory.Exists(formsFolder))
            {
                MessageBox.Show("Formlar klasörü geçerli değil!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Hedef klasörü kontrol et ve oluştur
            if (!Directory.Exists(targetFolder))
            {
                try
                {
                    Directory.CreateDirectory(targetFolder);
                    Log($"Hedef klasör oluşturuldu: {targetFolder}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hedef klasör oluşturulurken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            var formFiles = Directory.GetFiles(formsFolder, "*.cs", SearchOption.AllDirectories)
                                      .Where(f => !f.EndsWith(".Designer.cs"))  // Designer dosyaları hariç
                                      .ToList();

            foreach (var formFile in formFiles)
            {
                try
                {
                    string formName = Path.GetFileNameWithoutExtension(formFile);
                    Form formInstance = CreateFormInstance(formName, formFile);

                    if (formInstance != null)
                    {
                        ResxGenerator.GenerateResx(formInstance, targetFolder);
                        Log($"{formName} için RESX üretildi.");
                        formInstance.Dispose();
                    }
                    else
                    {
                        Log($"{formName} formu oluşturulamadı.");
                    }
                }
                catch (Exception ex)
                {
                    Log($"Hata ({formFile}): {ex.Message}");
                }
            }

            MessageBox.Show("Tüm işlemler tamamlandı!", "Bitti", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }





        private void btnSelectTargetFolder_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBoxTargetFolder.Text = dialog.SelectedPath;
                    Log($"Hedef klasör seçildi: {dialog.SelectedPath}");
                }
            }
        }

        private void btnSelectFormsFolder_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBoxFormsFolder.Text = dialog.SelectedPath;
                    Log($"Formlar klasörü seçildi: {dialog.SelectedPath}");
                }
            }
        }
        private void btnSelectDllFolder_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBoxDllFolder.Text = dialog.SelectedPath;
                    Log($"Dll klasörü seçildi: {dialog.SelectedPath}");
                }
            }
        }

        //private void btnRun_Click(object sender, EventArgs e)
        //{
        //    string formsFolder = textBoxFormsFolder.Text;
        //    string targetFolder = textBoxTargetFolder.Text;

        //    if (!Directory.Exists(formsFolder))
        //    {
        //        MessageBox.Show("Formlar klasörü geçerli değil!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    // Check if the target folder exists, if not, create it
        //    if (!Directory.Exists(targetFolder))
        //    {
        //        try
        //        {
        //            Directory.CreateDirectory(targetFolder);
        //            Log($"Hedef klasör oluşturuldu: {targetFolder}");
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"Hedef klasör oluşturulurken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }
        //    }


        //    var formFiles = Directory.GetFiles(formsFolder, "*.cs", SearchOption.AllDirectories)
        //                              .Where(f => f.EndsWith(".Designer.cs") == false) // Designer dosyaları hariç
        //                              .ToList();

        //    foreach (var formFile in formFiles)
        //    {
        //        try
        //        {
        //            string formName = Path.GetFileNameWithoutExtension(formFile);
        //            Form formInstance = CreateFormInstance(formName);

        //            if (formInstance != null)
        //            {
        //                ResxGenerator.GenerateResx(formInstance, targetFolder);
        //                Log($"{formName} için RESX üretildi.");
        //                formInstance.Dispose();
        //            }
        //            else
        //            {
        //                Log($"{formName} formu oluşturulamadı.");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Log($"Hata ({formFile}): {ex.Message}");
        //        }
        //    }

        //    MessageBox.Show("Tüm işlemler tamamlandı!", "Bitti", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //}


        private void MultiLang_ResGen_Load(object sender, EventArgs e)
        {
            // 'Properties.Settings.Default' ayarlarını oku
            textBoxTargetFolder.Text = Properties.Settings.Default.lastResxFolderPath ?? "";
            textBoxFormsFolder.Text = Properties.Settings.Default.lastFormsFolderPath ?? "";

            //Log("Ayarlar yüklendi.");
        }

        private void MultiLang_ResGen_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Settings'e kaydetme
            Properties.Settings.Default.lastResxFolderPath = textBoxTargetFolder.Text;
            Properties.Settings.Default.lastFormsFolderPath = textBoxFormsFolder.Text;
            Properties.Settings.Default.Save();  // Değişiklikleri kaydet
        }

    }
}