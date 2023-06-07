GRANT SELECT, INSERT, DELETE, UPDATE ON Users TO UserManager;
GRANT SELECT, INSERT, DELETE, UPDATE ON UserGroup TO GroupManager;
GRANT SELECT, INSERT, DELETE, UPDATE ON UserGroup TO ConnectManager;
GRANT SELECT, INSERT, DELETE, UPDATE ON Menu TO GroupManager;
GRANT SELECT ON UserMenu TO UserManager;
GRANT SELECT ON UserMenu TO GroupManager;
GRANT SELECT ON UserMenu TO ConnectManager;
GRANT INSERT, DELETE ON UserMenu TO GroupManager;
/*Neu trong comboBox bang du lieu cua Grant khong hien thi day du cac bang du lieu cua app thi thuc thi nhung dong duoi:
GRANT SELECT ON Menu TO ConnectManager;
GRANT SELECT ON Users TO ConnectManager; */

