# F# プログラム集

| パス | 概要 |
| --- | --- |
| `listRepos.fsx` | GitHub のリポジトリ一覧を取得して表示する F# スクリプト |
| `gtk/` | GtkSharp を用いた GUI アプリケーション |

## 注意点

- `listRepos.fsx` を実行する場合、事前に `.env` を作成し `GH_TOKEN` 環境変数の値を設定しておいてください。
- `gtk/` を Windows で実行する場合、GTK の共有ライブラリの導入が別途必要になります。
  - MSYS2 の場合、 `pacman -S mingw-w64-x86_64-gtk3` で導入できます。
