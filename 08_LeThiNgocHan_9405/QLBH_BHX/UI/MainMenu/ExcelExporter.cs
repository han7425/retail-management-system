using System;
using System.IO;
using System.Data;
using System.Windows.Forms; // Quan trọng: Để nhận diện DataGridView
using OfficeOpenXml;        // Thư viện EPPlus

public static class ExcelExporter
{
    // Hàm xuất Excel dùng chung cho mọi Form
    public static bool ExportToExcel(DataGridView dgv, string sheetName, string defaultFileName)
    {
        ExcelPackage.License.SetNonCommercialPersonal("StudentProject");
        if (dgv.Rows.Count == 0 || (dgv.Rows.Count == 1 && dgv.Rows[0].IsNewRow))
        {
            MessageBox.Show("Không có dữ liệu để xuất ra Excel!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        // 3. Mở hộp thoại chọn nơi lưu file
        SaveFileDialog saveFile = new SaveFileDialog();
        saveFile.Filter = "Excel Files (*.xlsx)|*.xlsx";
        saveFile.FileName = defaultFileName + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"); // Thêm giờ phút giây để tránh trùng

        if (saveFile.ShowDialog() == DialogResult.OK)
        {
            try
            {
                // 4. Khởi tạo file Excel
                using (ExcelPackage package = new ExcelPackage())
                {
                    // Tạo Sheet mới
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);

                    // --- Viết Tiêu đề Cột (Header) ---
                    for (int i = 0; i < dgv.Columns.Count; i++)
                    {
                        // Excel bắt đầu từ dòng 1, cột 1
                        worksheet.Cells[1, i + 1].Value = dgv.Columns[i].HeaderText;

                        // Tô đậm và canh giữa tiêu đề
                        worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                        worksheet.Cells[1, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    }

                    // --- Viết Dữ liệu (Data) ---
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        // Bỏ qua dòng trống cuối cùng (dòng NewRow để thêm mới)
                        if (dgv.Rows[i].IsNewRow) continue;

                        for (int j = 0; j < dgv.Columns.Count; j++)
                        {
                            // Lấy giá trị từ DataGridView
                            object cellValue = dgv.Rows[i].Cells[j].Value;

                            // Ghi vào Excel (Dòng bắt đầu từ 2 vì dòng 1 là Header)
                            worksheet.Cells[i + 2, j + 1].Value = (cellValue != null && cellValue != DBNull.Value)
                                                                  ? cellValue.ToString()
                                                                  : string.Empty;

                            // Nếu muốn định dạng ngày tháng tự động (Tùy chọn)
                            // if (cellValue is DateTime) 
                            //     worksheet.Cells[i + 2, j + 1].Style.Numberformat.Format = "dd/MM/yyyy";
                        }
                    }

                    // --- Tự động căn chỉnh độ rộng cột ---
                    worksheet.Cells.AutoFitColumns();

                    // 5. Lưu file xuống ổ cứng
                    File.WriteAllBytes(saveFile.FileName, package.GetAsByteArray());

                    MessageBox.Show("Xuất dữ liệu ra Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Lỗi: File Excel này đang được mở. Vui lòng đóng nó lại trước khi xuất đè!", "Lỗi file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi xuất Excel: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        return false; // Người dùng bấm Cancel
    }
}