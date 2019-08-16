DELIMITER $$
USE `FinancialDataAnalysis`$$

DROP PROCEDURE IF EXISTS EARS_EARSMain_SP_Temp_DML$$
CREATE PROCEDURE EARS_EARSMain_SP_Temp_DML()
BEGIN
	IF NOT EXISTS (select 1 from information_schema.tables where table_schema='FinancialDataAnalysis' and table_name='Account'  )
	THEN
		create table Account
		(
		   ExcelRecordId        int comment '对应的excel',
		   AccountCode          varchar(30) comment '科目编号',
		   ParentCode           varchar(30) comment '父级编号',
		   AccountName          varchar(30) comment '科目名称',
		   AccountLevel         tinyint comment '科目层级',
		   AccountTypeIdl       tinyint comment '类型：1：收入，2：支出',
		   AccountSumTypeId     tinyint comment '相加类型：1：+  2：-',
		   IsLeaf               tinyint comment '是否叶子',
           index IX_Account_ExcelRecordId(ExcelRecordId)
		) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8 COMMENT='科目';
	end if;
    
    IF NOT EXISTS (select 1 from information_schema.tables where table_schema='FinancialDataAnalysis' and table_name='ExcelRecord'  )
	THEN
		create table ExcelRecord
		(
		   ExcelRecordId        int not null auto_increment comment '主键',
		   ExcelName            varchar(200) comment 'excel名称',
		   CreateDateTime       datetime comment '创建日期',
		   ExcelUrl             varchar(500) comment 'excel地址',
		   StatusFlag           tinyint comment '状态',
		   primary key (ExcelRecordId)
		) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8 COMMENT='excel导入记录';
	end if;
    
    IF NOT EXISTS (select 1 from information_schema.tables where table_schema='FinancialDataAnalysis' and table_name='FinancialDataItem'  )
	THEN
		create table FinancialDataItem
		(
		   ExcelRecordId        int comment '对应的excel',
		   ItemId               int comment '项目Id',
		   ParentId             int comment '父级Id',
		   ItemName             varchar(100) comment '项目名称',
		   ItemLevel            int comment '项目层级',
		   IsLeaf               int comment '是否叶子',
		   ItemTypeId           int comment '类型：1：虚拟集团 2：事业部，3：片区，4：性质，5：项目',
           index IX_FinancialDataItem_ExcelRecordId(ExcelRecordId)
		) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8 COMMENT='项目集合';
	end if;

	IF NOT EXISTS (select 1 from information_schema.tables where table_schema='FinancialDataAnalysis' and table_name='FinancialData'  )
	THEN
		create table FinancialData
		(
		   FinancialDataId      int not null auto_increment,
		   ExcelRecordId        int comment '所属excel',
		   XiangMuIdId          int comment '项目Id',
		   XingZhiId            int comment '性质',
		   PianQuId             int comment '片区',
		   ShiYeBuId            int comment '事业部',
		   AccountCode          int comment '科目Id',
		   QiJianTypeId         int comment '会计期间：1：本年，2：本月',
		   Data                 double comment '数据',
		   primary key (FinancialDataId)
		) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8 COMMENT='数据集合';
	end if;
    
END$$

CALL EARS_EARSMain_SP_Temp_DML()$$
DROP PROCEDURE IF EXISTS EARS_EARSMain_SP_Temp_DML$$


DROP PROCEDURE IF EXISTS Account_Get$$
CREATE PROCEDURE `Account_Get`(

)
    SQL SECURITY INVOKER
BEGIN
/* 
创建者:		高峰
创建日期:	2019-08-16
描述：		获取所有科目	
修改记录:
--------------------------------------------------------
修改者		修改日期		修改目的

--------------------------------------------------------
*/
	SELECT 
		*
	FROM `Account`;
END$$
