var deleteExcelColumnForm = Ext.create('Ext.form.Panel', {
    bodyStyle: 'padding:5 5 5 5',//表单边距
    frame: true,
    buttonAlign: 'center',
    defaults: {//统一设置表单字段默认属性
        labelSeparator: '：',//分隔符
        labelWidth: 80,//标签宽度
        allowBlank: false,//是否允许为空
        labelAlign: 'left',//标签对齐方式
        msgTarget: 'side'   //在字段的右边显示一个提示信息
    },
    items: [{
        xtype: 'textfield',
        name: 'KeyWordName',
        fieldLabel: '文字名称',
        anchor: '95%'
    }, {
        xtype: 'textfield',
        name: 'RowIndex',
        fieldLabel: '所在行',
        anchor: '95%'
    }, {
        xtype: 'filefield',
        name: 'ExcelFile',
        fieldLabel: '选择文件',
        anchor: '95%',
        buttonText: '选择Excel'
    }],
    buttons: [{
        text: '开始执行',
        handler: function () {
            var form = deleteExcelColumnForm.getForm();
            if (form.isValid()) {
                uploadExcelWin.hide();
                form.submit({
                    url: '/FinancialData/DeleteExcelColumn',
                    waitMsg: '正在执行，请稍候...',
                    success: function (fp, o) {
                        deleteExcelColumnWin.hide();
                        location.href = o.result.FilePath;
                    },
                    failure: function () {
                        Ext.Msg.alert('提示信息', '执行失败，请查看错误日志!');
                    }
                });
            }
        }
    }]
});

var deleteExcelColumnWin = Ext.create("Ext.window.Window",
    {
        title: '删除Excel列',
        modal: true,
        constrainHeader: true,
        resizable: false,
        height: 160,
        width: 550,
        layout: "fit",
        items: [deleteExcelColumnForm],
        closeAction: 'hide', //close 关闭  hide  隐藏
        listeners: {
            'afterrender': function () { },

            'show': function () {
                deleteExcelColumnForm.form.reset();
            },
            'hide': function () {

            }
        }
    });

