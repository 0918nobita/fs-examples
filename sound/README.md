# F# ではじめる音のプログラミング

OpenAL と波の知識を活用して、F# でいろいろな音を作ってみる

### Arch linux

```bash
pacman -S gcc openal
```

### Windows

MSYS2 の利用を想定しています

```bash
pacman -S mingw-w64-x86_64-gcc mingw-w64-x86_64-openal
```

## ビルド

```bash
dotnet fsi build.fsx
```

### 生成物

``build/libmysound.so`` : メインの F# プログラムから利用する共有ライブラリ

## 実行

```bash
dotnet run -p src/main # すでにビルド済みなら --no-build でビルドをスキップできる
```

## Lint

```bash
dotnet fsi lint.fsx
```

## Format

```bash
dotnet fsi format.fsx
```
