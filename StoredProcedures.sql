/****** Object:  StoredProcedure [dbo].[topics_CreateNode]    Script Date: 07/25/2009 12:50:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[topics_CreateNode]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[topics_CreateNode]
GO
/****** Object:  StoredProcedure [dbo].[topics_DeleteTree]    Script Date: 07/25/2009 12:50:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[topics_DeleteTree]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[topics_DeleteTree]
GO
/****** Object:  StoredProcedure [dbo].[topics_EditAttribute]    Script Date: 07/25/2009 12:50:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[topics_EditAttribute]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[topics_EditAttribute]
GO
/****** Object:  StoredProcedure [dbo].[topics_GenerateNestedSet]    Script Date: 07/25/2009 12:50:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[topics_GenerateNestedSet]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[topics_GenerateNestedSet]
GO
/****** Object:  StoredProcedure [dbo].[topics_GetAttributes]    Script Date: 07/25/2009 12:50:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[topics_GetAttributes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[topics_GetAttributes]
GO
/****** Object:  StoredProcedure [dbo].[topics_GetAttributesForTopic]    Script Date: 07/25/2009 12:50:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[topics_GetAttributesForTopic]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[topics_GetAttributesForTopic]
GO
/****** Object:  StoredProcedure [dbo].[topics_GetNodes]    Script Date: 07/25/2009 12:50:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[topics_GetNodes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[topics_GetNodes]
GO
/****** Object:  StoredProcedure [dbo].[topics_MoveTree]    Script Date: 07/25/2009 12:50:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[topics_MoveTree]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[topics_MoveTree]
GO
/****** Object:  StoredProcedure [dbo].[topics_UpdateNode]    Script Date: 07/25/2009 12:50:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[topics_UpdateNode]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[topics_UpdateNode]
GO
/****** Object:  StoredProcedure [dbo].[topics_CreateAttribute]    Script Date: 07/25/2009 12:50:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[topics_CreateAttribute]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[topics_CreateAttribute]
GO
/****** Object:  StoredProcedure [dbo].[topics_CreateAttribute]    Script Date: 07/25/2009 12:50:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[topics_CreateAttribute]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-----------------------------------------------------------------------------------------------------------------------------------------------
-- PROCEDURE: topics_CreateAttribute
--
-- Purpose:	Creates a new attribute.
--
-- History:	John Mulhausen	06232009
-----------------------------------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [dbo].[topics_CreateAttribute]
	@AttributeTypeID int,
	@AttributeName nvarchar(124)
AS
BEGIN
	SET NOCOUNT ON;

-----------------------------------------------------------------------------------------------------------------------------------------------	
-- Get the max AttributeID int from the Attribute table and increment it to create the next ID. Insert into the table.
-----------------------------------------------------------------------------------------------------------------------------------------------	
	
	DECLARE @NewID int
	SET @NewID = (SELECT MAX(AttributeID) from dbo.topics_Attribute) + 1;
	
	INSERT into dbo.topics_Attribute VALUES (@NewID, @AttributeName, @AttributeTypeID);

	
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[topics_UpdateNode]    Script Date: 07/25/2009 12:50:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[topics_UpdateNode]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-----------------------------------------------------------------------------------------------------------------------------------------------
-- PROCEDURE: topics_UpdateNode
--
-- Purpose:	Used to update the attributes of a provided node
--
-- History:	Casey Margell		04062009		Initial Creation baseaed on code from Celko''s Sql For Smarties
-----------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[topics_UpdateNode]
	@NodeID int = -1,
	@Attributes varchar(1024) = '''',
	@ParentID int = -1
AS

--Delete all the existing attributes
delete topics_TopicAttributes where TopicID = @NodeID

--insert all the attributes from the nodes we changed
Insert 
Into	topics_TopicAttributes (	
			TopicID			,
			AttributeID		,	 
			AttributeValue
			) 
select	@NodeID			,
		topics_Attributes.AttributeID, 
		SUBSTRING(s, CHARINDEX('':'', s) + 1, LEN(s)) 
from	Split (@Attributes, '';'')
join	topics_Attributes
  on	topics_Attributes.AttributeName = SUBSTRING(s, 0, CHARINDEX('':'', s))


return @NodeID
' 
END
GO
/****** Object:  StoredProcedure [dbo].[topics_MoveTree]    Script Date: 07/25/2009 12:50:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[topics_MoveTree]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		John Mulhausen
-- Create date: 20090630
-- Description:	Moves a node and all its chidren to be a child of the specified node.
-- =============================================
CREATE PROCEDURE [dbo].[topics_MoveTree]
	@TopicID int,
	@ParentID int 
AS

-- Moves part of a nested set tree to another part.
-- Pass in the left of the child (from) and the left of the parent (to)

DECLARE @cleft int
DECLARE @cright int
DECLARE @pleft int 
DECLARE @pright int
DECLARE @leftbound int 
DECLARE @rightbound int
DECLARE @treeshift int 
DECLARE @cwidth int
DECLARE @leftrange int 
DECLARE @rightrange int

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

SELECT @cleft=RangeLeft, @cright=RangeRight FROM topics_Topics WHERE TopicID = @TopicID
SELECT @pleft=RangeLeft, @pright=RangeRight FROM topics_Topics WHERE TopicID = @ParentID

-- Determine which direction the node is being moved;
-- Populate the values for how much the boundaries need to be shifted for all nodes affected
IF @cleft > @pleft
BEGIN
SET @treeshift = @pleft - @cleft + 1;
SET @leftbound = @pleft+1;
SET @rightbound = @cleft-1;
SET @cwidth = @cright-@cleft+1;
SET @leftrange = @cright;
SET @rightrange = @pleft;
END
ELSE
BEGIN
SET @treeshift = @pleft - @cright;
SET @leftbound = @cright + 1;
SET @rightbound = @pleft;
SET @cwidth = @cleft-@cright-1;
SET @leftrange = @pleft+1;
SET @rightrange = @cleft;
END

-- Update boundaries for all nodes that are below the target if tree is being moved up or above the target if tree is being moved down 
UPDATE dbo.topics_Topics
	SET RangeLeft = 
		CASE
			WHEN (RangeLeft BETWEEN @leftbound AND @rightbound) THEN RangeLeft + @cwidth
			WHEN (RangeLeft BETWEEN @cleft AND @cright) THEN RangeLeft + @treeshift
			ELSE RangeLeft 
		END,
	RangeRight = 
		CASE
			WHEN (RangeRight BETWEEN @leftbound AND @rightbound) THEN RangeRight + @cwidth
			WHEN (RangeRight BETWEEN @cleft AND @cright) THEN RangeRight + @treeshift
			ELSE RangeRight 
		END
	WHERE (RangeLeft < @leftrange) OR (RangeRight > @rightrange);

Declare @AttributeID int

Select	@AttributeID = AttributeID
From	topics_Attributes
Where	AttributeName = ''ParentID''

UPDATE	dbo.topics_TopicAttributes
Set		AttributeValue = @ParentID
Where	TopicID = @TopicID
And		AttributeID = @AttributeID' 
END
GO
/****** Object:  StoredProcedure [dbo].[topics_GetNodes]    Script Date: 07/25/2009 12:50:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[topics_GetNodes]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-----------------------------------------------------------------------------------------------------------------------------------------------
-- PROCEDURE: topics_GetNodes
--
-- Purpose:	Gets the tree of current nodes rooted from the provided TopicID. 
-- If no TopicID is provided then the sproc returns everything under the topic
-- with the lowest id.
--
-- History:	Casey Margell		04062009		Initial Creation baseaed on code from Celko''s Sql For Smarties
--			Jeremy Caney		07192009		Added support for AttributeID lookup
-----------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[topics_GetNodes]
	@TopicID int = -1,
	@Depth int = -1,
	@TopicName varchar(64) = null
AS

-----------------------------------------------------------------------------------------------------------------------------------------------
-- Declare Variables
-----------------------------------------------------------------------------------------------------------------------------------------------
Declare @TopTopicID int

-----------------------------------------------------------------------------------------------------------------------------------------------
-- Get the TopicID of the node to use as our root.
-----------------------------------------------------------------------------------------------------------------------------------------------
if @TopicID > -1
  begin
	Set @TopTopicID = @TopicID
  end
else if (@TopicName != null) 
  begin
	Select	@TopTopicID = TopicID
	FROM	topics_TopicAttributes
	where	AttributeName = ''Name''
	and		AttributeValue like @TopicName
  end
else
  begin
	Select		top(1) @TopTopicID = TopicID
	From		topics_Topics
	Order By	TopicID
  end
  
  
-----------------------------------------------------------------------------------------------------------------------------------------------
-- Create Temporary Storage Table
-----------------------------------------------------------------------------------------------------------------------------------------------
Create Table #TempStorage (
	TopicID int,
	SortOrder int identity(1,1)
	)

-----------------------------------------------------------------------------------------------------------------------------------------------
-- If we''re selecting everything under the root node select the nodes
-----------------------------------------------------------------------------------------------------------------------------------------------
if @Depth = -1
  begin
	Insert Into #TempStorage (TopicID)
	SELECT		T1.TopicID
	FROM		topics_Topics as T1, topics_Topics as T2
	WHERE		T1.RangeLeft BETWEEN T2.RangeLeft AND T2.RangeRight
		AND		T2.TopicID = @TopTopicID
	Order By	T1.RangeLeft
  end
  
-----------------------------------------------------------------------------------------------------------------------------------------------
-- TODO: Add depth support to allow for getting children 2 levels deep for example
-----------------------------------------------------------------------------------------------------------------------------------------------
--else if @Depth > 0
--  begin
--    --TODO
--  end

-----------------------------------------------------------------------------------------------------------------------------------------------
-- Otherwise we only want the root node
-----------------------------------------------------------------------------------------------------------------------------------------------
else
  begin
	Insert Into #TempStorage (TopicID)
	Select		TopicID
	From		topics_Topics
	Where		TopicID = @TopicID
  end

-----------------------------------------------------------------------------------------------------------------------------------------------
-- The first return set is the list of topic ids
-----------------------------------------------------------------------------------------------------------------------------------------------
--select	* 
--from	#TempStorage

-----------------------------------------------------------------------------------------------------------------------------------------------
-- The second is the attributes for those topics, note that parent data is 
-- stored in the attributes so the calling object can build the tree that way
-----------------------------------------------------------------------------------------------------------------------------------------------
select		TopicAttributes.TopicID, 
			Attributes.AttributeName, 
			TopicAttributes.AttributeValue
from		topics_TopicAttributes as TopicAttributes
join		topics_Attributes as Attributes
	on		Attributes.AttributeID = TopicAttributes.AttributeID 
join		#TempStorage as Storage
	on		Storage.TopicID = TopicAttributes.TopicID
Order by	SortOrder, AttributeName
' 
END
GO
/****** Object:  StoredProcedure [dbo].[topics_GetAttributesForTopic]    Script Date: 07/25/2009 12:50:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[topics_GetAttributesForTopic]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[topics_GetAttributesForTopic]
	-- Add the parameters for the stored procedure here
	@TopicID int = -1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    
    /*
		This outer-join-using query assures that a properly grouped, ordered list of available attributes is returned.
		The result set is fed into an HTML form for value editing. 
    */
	SELECT DISTINCT CASE WHEN topic.TopicID=@TopicID THEN topic.AttributeValue ELSE '''' END AS AttributeValue, 
				attribute.AttributeID, 
				attribute.AttributeName, 
				attribute.AttributeTypeID,
				attribute.AttributePurpose,
				attType.AttributeTypeName
	FROM         dbo.topics_Attribute AS attribute INNER JOIN
						  dbo.topics_AttributeType AS attType ON attribute.AttributeTypeID = attType.AttributeTypeID LEFT OUTER JOIN
						  dbo.topics_TopicAttributes AS topic ON attribute.AttributeID = topic.AttributeID
	WHERE     (topic.TopicID = @TopicID OR
						  topic.TopicID IS NULL) AND (attribute.AttributeName <> ''ParentID'') OR
						  (attribute.AttributeName <> ''ParentID'') AND (attribute.AttributeID NOT IN
							  (SELECT     AttributeID
								FROM          dbo.topics_TopicAttributes
								WHERE      (TopicID = @TopicID)))
	ORDER BY attribute.AttributeID, attribute.AttributeName
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[topics_GetAttributes]    Script Date: 07/25/2009 12:50:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[topics_GetAttributes]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		John Mulhausen
-- Create date: 20090710
-- Description:	Returns List of Available Attributes
-- =============================================
CREATE PROCEDURE [dbo].[topics_GetAttributes]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Return a list of all user-modifiable attributes. 
	SELECT AttributeName, AttributePurpose, AttributeID, topics_AttributeTypes.AttributeTypeName FROM
	topics_Attributes
		INNER JOIN topics_AttributeTypes ON 
		topics_AttributeTypes.AttributeTypeID = topics_Attributes.AttributeTypeID
	
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[topics_GenerateNestedSet]    Script Date: 07/25/2009 12:50:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[topics_GenerateNestedSet]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-----------------------------------------------------------------------------------------------------------------------------------------------
-- PROCEDURE: GENERATE NESTED SET
--
-- Purpose	Creates an adjacency list using the _ParentID fields in topics_TopicAttributes then takes the newly created adjacency list and 
--			uses it to generate a nested set based table in topics_Topics.  Useful for recovering from a corrupted nested set model.
--
-- History	Jeremy Caney		050909		Created initial version based on Celko''s conversion model
-----------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[topics_GenerateNestedSet]
AS

SET IDENTITY_INSERT topics_topics ON

-----------------------------------------------------------------------------------------------------------------------------------------------
-- RECREATE TOPICS_HIERARCHY
-----------------------------------------------------------------------------------------------------------------------------------------------
-- Delete original content
delete 
from		Topics_Hierarchy

-- Insert data from topics_TopicAttributes
insert
into		topics_Hierarchy
Select		TopicID as TopicID, 
			AttributeValue as ParentID, 
			GETDATE() as DateAdded
from		topics_TopicAttributes 
where		AttributeName = ''ParentID''

-- Address root node
update		topics_Hierarchy
set			Parent_TopicID = null
where		Parent_TopicID = -1

-----------------------------------------------------------------------------------------------------------------------------------------------
-- Celko''s conversion model (not properly formatted; copied from Celko)
-----------------------------------------------------------------------------------------------------------------------------------------------
BEGIN
DECLARE @RangeLeft_RangeRight INTEGER, @topics_topics_pointer INTEGER, @max_RangeLeft_RangeRight INTEGER;

SET @max_RangeLeft_RangeRight = 2 * (SELECT COUNT(*) FROM topics_hierarchy);

INSERT INTO topics_topics (Stack_Top, TopicID, RangeLeft, RangeRight)
SELECT 1, TopicID, 1, @max_RangeLeft_RangeRight 
  FROM topics_hierarchy
 WHERE Parent_TopicID IS NULL;

SET @RangeLeft_RangeRight = 2;
SET @topics_topics_pointer = 1;

DELETE FROM topics_hierarchy 
 WHERE Parent_TopicID IS NULL;

-- The topics_topics is now loaded and ready to use

WHILE (@RangeLeft_RangeRight < @max_RangeLeft_RangeRight)
BEGIN 
 IF EXISTS (SELECT *
              FROM topics_topics AS S1, topics_hierarchy AS T1
             WHERE S1.TopicID = T1.Parent_TopicID
               AND S1.stack_top = @topics_topics_pointer)
    BEGIN -- push when stack_top has subordinates and set RangeLeft value
      INSERT INTO topics_topics (Stack_Top, TopicID, RangeLeft, RangeRight)
      SELECT (@topics_topics_pointer + 1), MIN(T1.TopicID), @RangeLeft_RangeRight, NULL
        FROM topics_topics AS S1, topics_hierarchy AS T1
       WHERE S1.TopicID = T1.Parent_TopicID
         AND S1.stack_top = @topics_topics_pointer;

      -- remove this row from topics_hierarchy 
      DELETE FROM topics_hierarchy
       WHERE TopicID = (SELECT TopicID
                        FROM topics_topics
                       WHERE stack_top = @topics_topics_pointer + 1);
      SET @topics_topics_pointer = @topics_topics_pointer + 1;
    END -- push
    ELSE
    BEGIN  -- pop the topics_topics and set RangeRight value
      UPDATE topics_topics
         SET RangeRight = @RangeLeft_RangeRight,
             stack_top = -stack_top
       WHERE stack_top = @topics_topics_pointer 
      SET @topics_topics_pointer = @topics_topics_pointer - 1;
    END; -- pop
  SET @RangeLeft_RangeRight = @RangeLeft_RangeRight + 1;
  END; -- if
END; -- while

SELECT * FROM topics_topics ORDER BY RangeLeft;

SET IDENTITY_INSERT topics_topics OFF
' 
END
GO
/****** Object:  StoredProcedure [dbo].[topics_EditAttribute]    Script Date: 07/25/2009 12:50:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[topics_EditAttribute]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-----------------------------------------------------------------------------------------------------------------------------------------------
-- PROCEDURE: topics_EditAttribute
--
-- Purpose:	Makes a change to an attribute. (Not to be confused with editing an attribute''s associated data, in AttributeValue)
--
-- History:	John Mulhausen	06232009
-----------------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[topics_EditAttribute]
	@AttributeID int,
	@NewAttributeTypeID int,
	@NewAttributeName nvarchar(124)
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE dbo.topics_Attributes
	SET AttributeName = @NewAttributeName, AttributeTypeID = @NewAttributeTypeID
	WHERE topics_Attributes.AttributeID = @AttributeID
	
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[topics_DeleteTree]    Script Date: 07/25/2009 12:50:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[topics_DeleteTree]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		John Mulhausen
-- Create date: 20090630
-- Description:	Deletes a tree from inside a nested set. Probably obsoletes DeleteNode.
-- =============================================
CREATE PROCEDURE [dbo].[topics_DeleteTree]
	-- Add the parameters for the stored procedure here
	@TopicID int
AS
DECLARE @myLeft int
DECLARE @myRight int
DECLARE @myWidth int

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT @myLeft = RangeLeft, @myRight = RangeRight, @myWidth = RangeRight - RangeLeft + 1
	FROM topics_Topics
	WHERE TopicID = @TopicID;

	-- Delete attributes and entries for topic and all children.
	
	DELETE FROM topics_TopicAttributes WHERE TopicID IN (SELECT TopicID from topics_Topics WHERE RangeLeft BETWEEN @myLeft AND @myRight);
	DELETE FROM topics_Topics WHERE RangeLeft BETWEEN @myLeft AND @myRight;

	-- Shift position of other nodes to take tree''s place.
	UPDATE topics_Topics SET RangeRight = RangeRight - @myWidth WHERE RangeRight > @myRight;
	UPDATE topics_Topics SET RangeLeft = RangeLeft - @myWidth WHERE RangeLeft > @myRight;

END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[topics_CreateNode]    Script Date: 07/25/2009 12:50:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[topics_CreateNode]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-----------------------------------------------------------------------------------------------------------------------------------------------
-- PROCEDURE: topics_CreateNode
--
-- Purpose:	Used to create a new Topic node
--
-- History:	Casey Margell		04062009		Initial Creation baseaed on code from Celko''s Sql For Smarties
-----------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[topics_CreateNode]
	@ParentID int = -1,
	@Attributes varchar(max) = ''''
AS

-----------------------------------------------------------------------------------------------------------------------------------------------
-- Declare and set varialbes
-----------------------------------------------------------------------------------------------------------------------------------------------
DECLARE @rms Integer --Right Most Sibling
SET		@rms = 0

-----------------------------------------------------------------------------------------------------------------------------------------------
-- If we''re adding a node with a parent then we need to make space for it first.
-----------------------------------------------------------------------------------------------------------------------------------------------
if (@ParentID > -1) 
begin 
	Select	@rms = RangeRight
	From	topics_topics
	Where	TopicID = @ParentID

	UPDATE	topics_Topics
	SET		RangeLeft = 
		CASE
			WHEN RangeLeft > @rms
				THEN RangeLeft + 2
			ELSE RangeLeft
		END,
			RangeRight = 
		CASE 
			WHEN RangeRight >= @rms
				THEN RangeRight + 2
			ELSE RangeRight 
		END
	 WHERE RangeRight >= @rms
end

-----------------------------------------------------------------------------------------------------------------------------------------------
-- Create the new node and store it''d ID
-----------------------------------------------------------------------------------------------------------------------------------------------
INSERT INTO topics_topics (RangeLeft, RangeRight)
VALUES (@rms, (@rms + 1))

Declare @NewNode int

select @NewNode = @@IDENTITY

-----------------------------------------------------------------------------------------------------------------------------------------------
-- Create attributes from the provided string
-----------------------------------------------------------------------------------------------------------------------------------------------
Insert 
Into	topics_TopicAttributes (	
			TopicID			,
			AttributeID		,	 
			AttributeValue
			) 
select	@NewNode			,
		topics_Attributes.AttributeID, 
		SUBSTRING(s, CHARINDEX('':'', s) + 1, LEN(s)) 
from	Split (@Attributes, '';'')
join	topics_Attributes
  on	topics_Attributes.AttributeName = SUBSTRING(s, 0, CHARINDEX('':'', s))
Where	s not like ''ParentID:%''

Declare	@ParentID_AttributeID int

Select	@ParentID_AttributeID = AttributeID
From	topics_Attributes 
Where	AttributeName = ''ParentID''

print @ParentID_AttributeID

Insert Into	topics_TopicAttributes (TopicID, AttributeID, AttributeValue)
values (@NewNode, @ParentID_AttributeID, @ParentID)

return @NewNode
' 
END
GO
