
    var uploadExcelForm = Ext.create('Ext.form.Panel', {
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
            xtype:'textfield',
            name: 'ExcelName',
            fieldLabel: '文件名称',
            anchor: '95%'
        },{
            xtype: 'filefield',
            name: 'ExcelFile',
            fieldLabel: '选择文件',
            anchor: '95%',
            buttonText: '选择Excel'
        }],
        buttons: [{
            text: '上传文件',
            handler: function () {
                var form = uploadExcelForm.getForm();
                if (form.isValid()) {
                    uploadExcelWin.hide();
                    form.submit({
                        url: '/FinancialData/UpladExcel',
                        waitMsg: '正在上传，请稍候...',
                        success: function (fp, o) {
                            Ext.getCmp("ExcelListGrid").store.load();
                            Ext.Msg.alert('提示信息', '已经成功上传!');
                        },
                        failure: function() {
                            Ext.Msg.alert('提示信息', '上传失败，请查看错误日志!');
                        }
                    });
                }
            }
        }]
    });

    var uploadExcelWin = Ext.create("Ext.window.Window",
        {
            title: '上传Excel',
            modal: true,
            constrainHeader: true,
            resizable: false,
            height: 130,
            width: 550,
            layout: "fit",
            items: [uploadExcelForm],
            closeAction: 'hide', //close 关闭  hide  隐藏
            listeners: {
                'afterrender': function() {},

                'show': function() {
                    uploadExcelForm.form.reset();
                },
                'hide': function() {
                    
                }
            }
        });
    
