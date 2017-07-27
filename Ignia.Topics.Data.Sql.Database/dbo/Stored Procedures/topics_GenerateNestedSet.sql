﻿-----------------------------------------------------------------------------------------------------------------------------------------------
-- PROCEDURE	GENERATE NESTED SET
--
-- Purpose	Creates an adjacency list using the _ParentID fields in topics_TopicAttributes then takes the newly created adjacency list and 
--		uses it to generate a nested set based table in topics_Topics.  Useful for recovering from a corrupted nested set model.
--
-- History	Jeremy Caney		050909  Created initial version based on Celko's conversion model
-----------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[topics_GenerateNestedSet]
AS

SET IDENTITY_INSERT topics_topics ON

-----------------------------------------------------------------------------------------------------------------------------------------------
-- RECREATE TOPICS_HIERARCHY
-----------------------------------------------------------------------------------------------------------------------------------------------
-- Delete original content
DELETE
FROM		Topics_Hierarchy

-- Insert data from topics_TopicAttributes
INSERT
INTO		topics_Hierarchy
SELECT		TopicID			AS TopicID, 
		AttributeValue		AS ParentID, 
		GETDATE()		AS DateAdded
FROM		topics_TopicAttributes 
WHERE		AttributeID		= 2

-- Address root node
UPDATE		topics_Hierarchy
SET		Parent_TopicID		= null
WHERE		Parent_TopicID		= -1

-----------------------------------------------------------------------------------------------------------------------------------------------
-- Celko's conversion model (not properly formatted; copied from Celko)
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