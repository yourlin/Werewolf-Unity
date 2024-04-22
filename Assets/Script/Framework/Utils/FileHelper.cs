using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class FileHelper
{
    public static void AppendTextToFile (string filePath, string text) {
        try {

            // 检查文件是否存在
            if (!File.Exists (filePath)) {
                // 如果文件不存在,则创建新文件
                using (File.Create (filePath)) { }
            }

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

    public static void ClearFile (string filePath) {
        try {
            // 使用 StreamWriter 以写入模式打开文件
            using (StreamWriter writer = new StreamWriter (filePath, false)) {
                // 不写入任何内容,从而清空文件
                writer.Write ("");
            }
        } catch (IOException ex) {
            // 处理异常
            Console.WriteLine ($"An error occurred while clearing the file: {ex.Message}");
        }
    }

    public static string[] ReadFromFile (string filePath) {
        try {
            // 读取文件内容
            string fileContent = File.ReadAllText (filePath);

            // 将文件内容按行分割
            return fileContent.Split (new [] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        } catch (IOException ex) {
            // 处理异常
            Console.WriteLine ($"An error occurred while reading the history file: {ex.Message}");
            return null;
        }

    }
}

