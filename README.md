# CsDCtoGitMD
# 概要
C#のドキュメントコメントをGit用のマークダウンテキストに変換します。
# 詳細
「///」で自動生成されるコメントと関数を自動でMD形式に変換します。
# 使用方法
- VS2022でビルドしています。
- 左側のテキストボックスにDocumentCommentをペーストすると自動的に変換され右のテキストボックスに出力されます。
- クリップボードにコピーを押すと、生成されたテキストがコピーされます。
![image](https://github.com/user-attachments/assets/6efdcf81-efd8-4aa7-a512-efeede66ea21)

# 問題点
- public static関数にしか対応していません。
- public staticでラッピングした関数用に作成したもののため、長い関数等はうまく動きません。（関数内に{}があると壊れそう）
