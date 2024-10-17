class classname : public superclassname {
protected:
  // instance variables

public:
  // Class (static) functions
  static void *classMethod1();
  static return_type classMethod2();
  static return_type classMethod3(param1_type param1_varName);

  // Instance (member) functions
  return_type instanceMethod1With1Parameter(param1_type param1_varName);
  return_type
  instanceMethod2With2Parameters(param1_type param1_varName,
                                 param2_type param2_varName = default);
};

@interface classname : superclassname {
  // instance variables
}
+ classMethod1;
+ (return_type)classMethod2;
+ (return_type)classMethod3:(param1_type)param1_varName;

- (return_type)instanceMethod1With1Parameter:(param1_type)param1_varName;
- (return_type)instanceMethod2With2Parameters:(param1_type)param1_varName
                              param2_callName:(param2_type)param2_varName;
@end
