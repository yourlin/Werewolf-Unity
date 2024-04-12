using System;
using System.IO;

public class FileHelper
{
    public static void AppendTextToFile (string filePath, string text) {
        try {
            // 使用 StreamWriter 以追加模式打开文件
            using (StreamWriter writer = new StreamWriter (filePath, true)) {
                // 写入新的文本
                writer.WriteLine (text);
            }
        } catch (IOException ex) {
            // 处理异常
            Console.WriteLine ($"An error occurred while appending text to the file: {ex.Message}");
        }
    }
}

