# SqlCe2SQLite

Copy Data from a SqlCE-Database (.sdf) to a SQLite-Database (.db3)

## Info:
* both Databases must be exist.
* both Tables (Structure) must be exist.

## Timings (on Microsoft Surface Book i7, 16GB, SSD):
### V2: mit Bulk insert
* Count: Tables: 22, Rows: 83935, Rec/Sec: 6044,44
* Duration: 00:00:13.88

### V1: normal
* Count: Tables: 22, Rows: 83935, Rec/Sec: 36,37
* Duration: 00:38:27.48
