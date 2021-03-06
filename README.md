# MongoDB FTDC Parser

The goal of this project is to provide a utility to parse the contents of the [`diagnostic.data`](https://docs.mongodb.com/manual/reference/parameters/#param.diagnosticDataCollectionDirectoryPath) files generated by MongoDB. These data files contain [Full Time Diagnostic Data Capture](https://docs.mongodb.com/manual/administration/analyzing-mongodb-performance/#full-time-diagnostic-data-capture), which can consists of compressed samples of internal MongoDB command output and host machine stats.

This project uses [zlibnet](https://github.com/gdalsnes/zlibnet) to decompress zlib compressed data.

## Roadmap

- [x] Load FTDC from file
- [ ] Load FTDC files from a directory
- [x] Decompress FTDC data
- [ ] CLI: summarize loaded FDTC file(s)
	- [ ] timespan of loaded files
	- [ ] system details from ftdc?

## Development

```
git submodule init
git submodule update
```

## Usage

TODO: CLI How To

## Importing FTDC into Grafana

TODO