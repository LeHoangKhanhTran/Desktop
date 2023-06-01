CREATE PROCEDURE PCR_FINDSTRING @userGroupID VARCHAR(7), @result NVARCHAR(255) OUTPUT, @form NVARCHAR(255) OUTPUT
AS
BEGIN
  DECLARE @id VARCHAR(7), @string NVARCHAR(255), @formName NVARCHAR(255);
  DECLARE myCursor CURSOR FOR SELECT GroupID, ConnectString, UserForm FROM UserGroup;
  OPEN myCursor 
  FETCH NEXT FROM myCursor INTO @id, @string, @formName;
  SET @result = 'None'
  WHILE @@FETCH_STATUS = 0 AND @result = 'None'
    BEGIN  
      if @id = @userGroupID
        BEGIN
		  SET @result = @string;
		  SET @form = @formName;
		END	
	FETCH NEXT FROM myCursor INTO @id, @string, @formName;
	END
CLOSE myCursor
DEALLOCATE myCursor
END